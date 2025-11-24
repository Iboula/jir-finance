# Deployment manuel etape par etape
$env:Path = [System.Environment]::GetEnvironmentVariable("Path","Machine") + ";" + [System.Environment]::GetEnvironmentVariable("Path","User")

$resourceGroup = "rg-jir-finance"
$location = "canadacentral"
$envName = "jir-finance-env"

Write-Host "=== Etape 1: Creation de l'environnement Container Apps ===" -ForegroundColor Cyan

az containerapp env create `
    --name $envName `
    --resource-group $resourceGroup `
    --location $location

Write-Host ""
Write-Host "Environnement cree avec succes!" -ForegroundColor Green
Write-Host ""
Write-Host "=== Etape 2: Recuperation des credentials ACR ===" -ForegroundColor Cyan

$acrUser = az acr credential show --name rejirfinance --query "username" --output tsv
$acrPass = az acr credential show --name rejirfinance --query "passwords[0].value" --output tsv

Write-Host "Credentials recuperes" -ForegroundColor Green
Write-Host ""
Write-Host "=== Etape 3: Deploiement PostgreSQL ===" -ForegroundColor Cyan

az containerapp create `
    --name jir-postgres `
    --resource-group $resourceGroup `
    --environment $envName `
    --image postgres:16-alpine `
    --target-port 5432 `
    --ingress internal `
    --cpu 0.5 --memory 1Gi `
    --min-replicas 1 --max-replicas 1 `
    --env-vars POSTGRES_USER=jir_user POSTGRES_PASSWORD=jir_password POSTGRES_DB=jir_db

Write-Host ""
Write-Host "PostgreSQL deploye! Attente 30s..." -ForegroundColor Green
Start-Sleep -Seconds 30

$pgUrl = az containerapp show --name jir-postgres --resource-group $resourceGroup --query "properties.configuration.ingress.fqdn" --output tsv
Write-Host "PostgreSQL URL: $pgUrl" -ForegroundColor Yellow
Write-Host ""
Write-Host "=== Etape 4: Deploiement API ===" -ForegroundColor Cyan

$connStr = "Host=$pgUrl;Port=5432;Database=jir_db;Username=jir_user;Password=jir_password"

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
    --env-vars ASPNETCORE_ENVIRONMENT=Production ASPNETCORE_URLS=http://+:8080 "ConnectionStrings__DefaultConnection=$connStr"

Write-Host ""
$apiUrl = az containerapp show --name jir-api --resource-group $resourceGroup --query "properties.configuration.ingress.fqdn" --output tsv
Write-Host "API deployee: https://$apiUrl" -ForegroundColor Green
Write-Host ""
Write-Host "=== Etape 5: Deploiement Blazor ===" -ForegroundColor Cyan

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
    --env-vars ASPNETCORE_ENVIRONMENT=Production ASPNETCORE_URLS=http://+:8080 "ApiBaseUrl=https://$apiUrl"

Write-Host ""
$blazorUrl = az containerapp show --name jir-blazor --resource-group $resourceGroup --query "properties.configuration.ingress.fqdn" --output tsv
Write-Host "Blazor deploye: https://$blazorUrl" -ForegroundColor Green
Write-Host ""
Write-Host "============================================================" -ForegroundColor Green
Write-Host "DEPLOIEMENT TERMINE!" -ForegroundColor Green
Write-Host "============================================================" -ForegroundColor Green
Write-Host ""
Write-Host "URLS:" -ForegroundColor Cyan
Write-Host "  Blazor:  https://$blazorUrl" -ForegroundColor White
Write-Host "  API:     https://$apiUrl" -ForegroundColor White
Write-Host "  DB:      $pgUrl (interne)" -ForegroundColor Gray
Write-Host ""
Write-Host "Testez l'application: https://$blazorUrl" -ForegroundColor Yellow
