# Deployment asynchrone avec no-wait
$env:Path = [System.Environment]::GetEnvironmentVariable("Path","Machine") + ";" + [System.Environment]::GetEnvironmentVariable("Path","User")

$resourceGroup = "rg-jir-finance"
$location = "canadacentral"
$envName = "jir-finance-env"

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Deploiement JIR Finance - Azure Container Apps" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Etape 1: Creer l'environnement
Write-Host "[1/5] Creation de l'environnement Container Apps..." -ForegroundColor Yellow
Write-Host "      (Cette operation peut prendre 3-5 minutes)" -ForegroundColor Gray

$envCheck = az containerapp env show --name $envName --resource-group $resourceGroup 2>$null
if (-not $envCheck) {
    az containerapp env create `
        --name $envName `
        --resource-group $resourceGroup `
        --location $location `
        --output none
    
    if ($LASTEXITCODE -ne 0) {
        Write-Host "Erreur lors de la creation de l'environnement" -ForegroundColor Red
        exit 1
    }
}

Write-Host "      Environnement pret!" -ForegroundColor Green
Write-Host ""

# Etape 2: Credentials ACR
Write-Host "[2/5] Recuperation des credentials Azure Container Registry..." -ForegroundColor Yellow
$acrUser = az acr credential show --name rejirfinance --query "username" --output tsv
$acrPass = az acr credential show --name rejirfinance --query "passwords[0].value" --output tsv

if (-not $acrUser -or -not $acrPass) {
    Write-Host "Erreur: Impossible de recuperer les credentials ACR" -ForegroundColor Red
    exit 1
}
Write-Host "      Credentials recuperes" -ForegroundColor Green
Write-Host ""

# Etape 3: PostgreSQL
Write-Host "[3/5] Deploiement PostgreSQL..." -ForegroundColor Yellow

$pgCheck = az containerapp show --name jir-postgres --resource-group $resourceGroup 2>$null
if (-not $pgCheck) {
    az containerapp create `
        --name jir-postgres `
        --resource-group $resourceGroup `
        --environment $envName `
        --image postgres:16-alpine `
        --target-port 5432 `
        --ingress internal `
        --cpu 0.5 --memory 1Gi `
        --min-replicas 1 --max-replicas 1 `
        --env-vars POSTGRES_USER=jir_user POSTGRES_PASSWORD=jir_password POSTGRES_DB=jir_db `
        --output none
    
    if ($LASTEXITCODE -ne 0) {
        Write-Host "Erreur lors du deploiement PostgreSQL" -ForegroundColor Red
        exit 1
    }
    
    Write-Host "      Attente du demarrage de PostgreSQL..." -ForegroundColor Gray
    Start-Sleep -Seconds 30
}

$pgUrl = az containerapp show --name jir-postgres --resource-group $resourceGroup --query "properties.configuration.ingress.fqdn" --output tsv
Write-Host "      PostgreSQL pret: $pgUrl" -ForegroundColor Green
Write-Host ""

# Etape 4: API
Write-Host "[4/5] Deploiement API..." -ForegroundColor Yellow
$connStr = "Host=$pgUrl;Port=5432;Database=jir_db;Username=jir_user;Password=jir_password"

$apiCheck = az containerapp show --name jir-api --resource-group $resourceGroup 2>$null
if (-not $apiCheck) {
    az containerapp create `
        --name jir-api `
        --resource-group $resourceGroup `
        --environment $envName `
        --image rejirfinance.azurecr.io/jir-api:latest `
        --target-port 8080 `
        --ingress external `
        --cpu 0.5 --memory 1Gi `
        --min-replicas 1 --max-replicas 3 `
        --registry-server rejirfinance.azurecr.io `
        --registry-username $acrUser `
        --registry-password $acrPass `
        --env-vars ASPNETCORE_ENVIRONMENT=Production ASPNETCORE_URLS=http://+:8080 "ConnectionStrings__DefaultConnection=$connStr" `
        --output none
    
    if ($LASTEXITCODE -ne 0) {
        Write-Host "Erreur lors du deploiement API" -ForegroundColor Red
        exit 1
    }
}

$apiUrl = az containerapp show --name jir-api --resource-group $resourceGroup --query "properties.configuration.ingress.fqdn" --output tsv
Write-Host "      API prete: https://$apiUrl" -ForegroundColor Green
Write-Host ""

# Etape 5: Blazor
Write-Host "[5/5] Deploiement Blazor..." -ForegroundColor Yellow

$blazorCheck = az containerapp show --name jir-blazor --resource-group $resourceGroup 2>$null
if (-not $blazorCheck) {
    az containerapp create `
        --name jir-blazor `
        --resource-group $resourceGroup `
        --environment $envName `
        --image rejirfinance.azurecr.io/jir-blazor:latest `
        --target-port 8080 `
        --ingress external `
        --cpu 0.5 --memory 1Gi `
        --min-replicas 1 --max-replicas 3 `
        --registry-server rejirfinance.azurecr.io `
        --registry-username $acrUser `
        --registry-password $acrPass `
        --env-vars ASPNETCORE_ENVIRONMENT=Production ASPNETCORE_URLS=http://+:8080 "ApiBaseUrl=https://$apiUrl" `
        --output none
    
    if ($LASTEXITCODE -ne 0) {
        Write-Host "Erreur lors du deploiement Blazor" -ForegroundColor Red
        exit 1
    }
}

$blazorUrl = az containerapp show --name jir-blazor --resource-group $resourceGroup --query "properties.configuration.ingress.fqdn" --output tsv
Write-Host "      Blazor pret: https://$blazorUrl" -ForegroundColor Green
Write-Host ""

# Resume final
Write-Host "========================================" -ForegroundColor Green
Write-Host "DEPLOIEMENT TERMINE AVEC SUCCES!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""
Write-Host "URLs de l'application:" -ForegroundColor Cyan
Write-Host ""
Write-Host "  Blazor App:  https://$blazorUrl" -ForegroundColor White
Write-Host "  API:         https://$apiUrl" -ForegroundColor White
Write-Host "  PostgreSQL:  $pgUrl (acces interne uniquement)" -ForegroundColor Gray
Write-Host ""
Write-Host "Actions suivantes:" -ForegroundColor Yellow
Write-Host "  1. Testez l'application: https://$blazorUrl" -ForegroundColor White
Write-Host "  2. Verifiez les logs si necessaire:" -ForegroundColor White
Write-Host "     az containerapp logs show --name jir-blazor --resource-group $resourceGroup --follow" -ForegroundColor Gray
Write-Host "  3. Surveillez dans le portail Azure:" -ForegroundColor White
Write-Host "     https://portal.azure.com/#resource/subscriptions/02d95495-5cb9-4c60-acdd-07b696db2508/resourceGroups/$resourceGroup" -ForegroundColor Gray
Write-Host ""
