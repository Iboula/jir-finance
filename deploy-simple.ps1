# Script de deploiement simplifie Azure Container Apps
# Ajouter Azure CLI au PATH
$env:Path += ";C:\Program Files\Microsoft SDKs\Azure\CLI2\wbin"

# Variables
$resourceGroup = "rg-jir-finance"
$location = "canadacentral"
$containerAppEnv = "jir-finance-env"
$acrName = "rejirfinance"
$acrLoginServer = "rejirfinance.azurecr.io"
$apiAppName = "jir-api"
$blazorAppName = "jir-blazor"
$postgresAppName = "jir-postgres"
$apiImage = "$acrLoginServer/jir-api:latest"
$blazorImage = "$acrLoginServer/jir-blazor:latest"
$postgresImage = "postgres:16-alpine"

Write-Host "Deploiement JIR Finance sur Azure Container Apps"
Write-Host ""

# Creer le groupe de ressources
Write-Host "Creation du groupe de ressources..."
az group create --name $resourceGroup --location $location --output none
Write-Host "Groupe de ressources cree: $resourceGroup"
Write-Host ""

# Creer l'environnement Container Apps
Write-Host "Creation de l'environnement Container Apps..."
az containerapp env create --name $containerAppEnv --resource-group $resourceGroup --location $location --output none
Write-Host "Environnement cree: $containerAppEnv"
Write-Host ""

# Recuperer les credentials ACR
Write-Host "Recuperation des credentials ACR..."
$acrUsername = az acr credential show --name $acrName --query "username" --output tsv
$acrPassword = az acr credential show --name $acrName --query "passwords[0].value" --output tsv
Write-Host "Credentials ACR recuperes"
Write-Host ""

# Deployer PostgreSQL
Write-Host "Deploiement PostgreSQL..."
az containerapp create `
    --name $postgresAppName `
    --resource-group $resourceGroup `
    --environment $containerAppEnv `
    --image $postgresImage `
    --target-port 5432 `
    --ingress internal `
    --cpu 0.5 --memory 1.0Gi `
    --min-replicas 1 --max-replicas 1 `
    --env-vars "POSTGRES_USER=jir_user" "POSTGRES_PASSWORD=jir_password" "POSTGRES_DB=jir_db" `
    --output none

Write-Host "PostgreSQL deploye"
Write-Host "Attente 30 secondes pour le demarrage..."
Start-Sleep -Seconds 30
Write-Host ""

# Obtenir l'URL PostgreSQL
$postgresUrl = az containerapp show --name $postgresAppName --resource-group $resourceGroup --query "properties.configuration.ingress.fqdn" --output tsv
Write-Host "PostgreSQL URL: $postgresUrl"
Write-Host ""

# Deployer API
Write-Host "Deploiement API..."
az containerapp create `
    --name $apiAppName `
    --resource-group $resourceGroup `
    --environment $containerAppEnv `
    --image $apiImage `
    --target-port 8080 `
    --ingress external `
    --cpu 0.5 --memory 1.0Gi `
    --min-replicas 1 --max-replicas 3 `
    --registry-server $acrLoginServer `
    --registry-username $acrUsername `
    --registry-password $acrPassword `
    --env-vars "ASPNETCORE_ENVIRONMENT=Production" "ASPNETCORE_URLS=http://+:8080" "ConnectionStrings__DefaultConnection=Host=$postgresUrl;Port=5432;Database=jir_db;Username=jir_user;Password=jir_password" `
    --output none

$apiUrl = az containerapp show --name $apiAppName --resource-group $resourceGroup --query "properties.configuration.ingress.fqdn" --output tsv
Write-Host "API deployee: https://$apiUrl"
Write-Host ""

# Deployer Blazor
Write-Host "Deploiement Blazor..."
az containerapp create `
    --name $blazorAppName `
    --resource-group $resourceGroup `
    --environment $containerAppEnv `
    --image $blazorImage `
    --target-port 8080 `
    --ingress external `
    --cpu 0.5 --memory 1.0Gi `
    --min-replicas 1 --max-replicas 3 `
    --registry-server $acrLoginServer `
    --registry-username $acrUsername `
    --registry-password $acrPassword `
    --env-vars "ASPNETCORE_ENVIRONMENT=Production" "ASPNETCORE_URLS=http://+:8080" "ApiBaseUrl=https://$apiUrl" `
    --output none

$blazorUrl = az containerapp show --name $blazorAppName --resource-group $resourceGroup --query "properties.configuration.ingress.fqdn" --output tsv
Write-Host "Blazor deploye: https://$blazorUrl"
Write-Host ""

# Resume
Write-Host "============================================================"
Write-Host "Deploiement termine!"
Write-Host "============================================================"
Write-Host ""
Write-Host "URLs:"
Write-Host "  PostgreSQL: $postgresUrl (interne)"
Write-Host "  API:        https://$apiUrl"
Write-Host "  Blazor:     https://$blazorUrl"
Write-Host ""
Write-Host "Pour voir les logs:"
Write-Host "  az containerapp logs show --name $apiAppName --resource-group $resourceGroup --follow"
Write-Host "  az containerapp logs show --name $blazorAppName --resource-group $resourceGroup --follow"
Write-Host ""
