# Script de déploiement Azure Container Apps pour JIR Finance
# Prérequis: Azure CLI installé et connecté (az login)

# Variables de configuration
$resourceGroup = "rg-jir-finance"
$location = "canadacentral"
$containerAppEnv = "jir-finance-env"
$acrName = "rejirfinance"
$acrLoginServer = "rejirfinance.azurecr.io"

# Noms des Container Apps
$apiAppName = "jir-api"
$blazorAppName = "jir-blazor"
$postgresAppName = "jir-postgres"

# Tags d'images
$apiImage = "$acrLoginServer/jir-api:latest"
$blazorImage = "$acrLoginServer/jir-blazor:latest"
$postgresImage = "postgres:16-alpine"

Write-Host "🚀 Déploiement de JIR Finance sur Azure Container Apps" -ForegroundColor Cyan
Write-Host ""

# Vérifier si Azure CLI est installé
$azVersion = Get-Command az -ErrorAction SilentlyContinue
if (-not $azVersion) {
    Write-Host "✗ Azure CLI n'est pas installé. Installez-le depuis: https://aka.ms/installazurecliwindows" -ForegroundColor Red
    exit 1
}
Write-Host "✓ Azure CLI détecté" -ForegroundColor Green

# Vérifier la connexion Azure
Write-Host "Vérification de la connexion Azure..." -ForegroundColor Yellow
$account = az account show 2>$null | ConvertFrom-Json
if (-not $account) {
    Write-Host "✗ Non connecté à Azure. Exécutez 'az login' d'abord." -ForegroundColor Red
    exit 1
}
Write-Host "✓ Connecté à Azure: $($account.name)" -ForegroundColor Green
Write-Host ""

# Créer le groupe de ressources si nécessaire
Write-Host "📦 Création du groupe de ressources..." -ForegroundColor Yellow
az group create `
    --name $resourceGroup `
    --location $location `
    --output none

Write-Host "✓ Groupe de ressources: $resourceGroup" -ForegroundColor Green

# Créer l'environnement Container Apps
Write-Host "🌍 Création de l'environnement Container Apps..." -ForegroundColor Yellow
$envExists = az containerapp env show `
    --name $containerAppEnv `
    --resource-group $resourceGroup `
    2>$null

if (-not $envExists) {
    az containerapp env create `
        --name $containerAppEnv `
        --resource-group $resourceGroup `
        --location $location `
        --output none
    Write-Host "✓ Environnement créé: $containerAppEnv" -ForegroundColor Green
} else {
    Write-Host "✓ Environnement existant: $containerAppEnv" -ForegroundColor Green
}

# Déployer PostgreSQL
Write-Host ""
Write-Host "🐘 Déploiement de PostgreSQL..." -ForegroundColor Yellow
az containerapp create `
    --name $postgresAppName `
    --resource-group $resourceGroup `
    --environment $containerAppEnv `
    --image $postgresImage `
    --target-port 5432 `
    --ingress internal `
    --cpu 0.5 `
    --memory 1.0Gi `
    --min-replicas 1 `
    --max-replicas 1 `
    --env-vars `
        "POSTGRES_USER=jir_user" `
        "POSTGRES_PASSWORD=jir_password" `
        "POSTGRES_DB=jir_db" `
    --output none

Write-Host "✓ PostgreSQL déployé" -ForegroundColor Green

# Attendre que PostgreSQL soit prêt
Write-Host "⏳ Attente du démarrage de PostgreSQL (30s)..." -ForegroundColor Yellow
Start-Sleep -Seconds 30

# Obtenir l'URL interne de PostgreSQL
$postgresUrl = az containerapp show `
    --name $postgresAppName `
    --resource-group $resourceGroup `
    --query "properties.configuration.ingress.fqdn" `
    --output tsv

Write-Host "✓ PostgreSQL URL: $postgresUrl" -ForegroundColor Green

# Déployer l'API
Write-Host ""
Write-Host "⚙️  Déploiement de l'API..." -ForegroundColor Yellow

# Récupérer les credentials ACR
$acrUsername = az acr credential show --name $acrName --query "username" --output tsv
$acrPassword = az acr credential show --name $acrName --query "passwords[0].value" --output tsv

az containerapp create `
    --name $apiAppName `
    --resource-group $resourceGroup `
    --environment $containerAppEnv `
    --image $apiImage `
    --target-port 8080 `
    --ingress external `
    --cpu 0.5 `
    --memory 1.0Gi `
    --min-replicas 1 `
    --max-replicas 3 `
    --registry-server $acrLoginServer `
    --registry-username $acrUsername `
    --registry-password $acrPassword `
    --env-vars `
        "ASPNETCORE_ENVIRONMENT=Production" `
        "ASPNETCORE_URLS=http://+:8080" `
        "ConnectionStrings__DefaultConnection=Host=$postgresUrl;Port=5432;Database=jir_db;Username=jir_user;Password=jir_password" `
    --output none

# Obtenir l'URL de l'API
$apiUrl = az containerapp show `
    --name $apiAppName `
    --resource-group $resourceGroup `
    --query "properties.configuration.ingress.fqdn" `
    --output tsv

Write-Host "✓ API déployée: https://$apiUrl" -ForegroundColor Green

# Déployer Blazor
Write-Host ""
Write-Host "🌐 Déploiement de l'application Blazor..." -ForegroundColor Yellow

az containerapp create `
    --name $blazorAppName `
    --resource-group $resourceGroup `
    --environment $containerAppEnv `
    --image $blazorImage `
    --target-port 8080 `
    --ingress external `
    --cpu 0.5 `
    --memory 1.0Gi `
    --min-replicas 1 `
    --max-replicas 3 `
    --registry-server $acrLoginServer `
    --registry-username $acrUsername `
    --registry-password $acrPassword `
    --env-vars `
        "ASPNETCORE_ENVIRONMENT=Production" `
        "ASPNETCORE_URLS=http://+:8080" `
        "ApiBaseUrl=https://$apiUrl" `
    --output none

# Obtenir l'URL de Blazor
$blazorUrl = az containerapp show `
    --name $blazorAppName `
    --resource-group $resourceGroup `
    --query "properties.configuration.ingress.fqdn" `
    --output tsv

Write-Host "✓ Blazor déployé: https://$blazorUrl" -ForegroundColor Green

# Résumé
Write-Host ""
Write-Host "=" * 60 -ForegroundColor Cyan
Write-Host "✅ Déploiement terminé avec succès!" -ForegroundColor Green
Write-Host "=" * 60 -ForegroundColor Cyan
Write-Host ""
Write-Host "📋 Résumé des URLs:" -ForegroundColor Cyan
Write-Host "  🐘 PostgreSQL: $postgresUrl (interne)" -ForegroundColor White
Write-Host "  ⚙️  API:        https://$apiUrl" -ForegroundColor White
Write-Host "  🌐 Blazor:     https://$blazorUrl" -ForegroundColor White
Write-Host ""
Write-Host "💡 Pour voir les logs:" -ForegroundColor Yellow
Write-Host "  az containerapp logs show --name $apiAppName --resource-group $resourceGroup --follow" -ForegroundColor Gray
Write-Host "  az containerapp logs show --name $blazorAppName --resource-group $resourceGroup --follow" -ForegroundColor Gray
Write-Host ""
Write-Host "🗑️  Pour supprimer les ressources:" -ForegroundColor Yellow
Write-Host "  az group delete --name $resourceGroup --yes --no-wait" -ForegroundColor Gray
Write-Host ""
