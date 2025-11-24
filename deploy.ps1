# Script de deploiement robuste avec reprise
$env:Path = [System.Environment]::GetEnvironmentVariable("Path","Machine") + ";" + [System.Environment]::GetEnvironmentVariable("Path","User")

$rg = "rg-jir-finance"
$env = "jir-finance-env"
$acrServer = "rejirfinance.azurecr.io"

Write-Host "=== DEPLOIEMENT JIR FINANCE ===" -ForegroundColor Cyan
Write-Host ""

# Credentials ACR
Write-Host "[Preparation] Recuperation credentials ACR..." -ForegroundColor Yellow
$acrUser = az acr credential show --name rejirfinance --query "username" -o tsv
$acrPass = az acr credential show --name rejirfinance --query "passwords[0].value" -o tsv
Write-Host "OK" -ForegroundColor Green
Write-Host ""

# === POSTGRESQL ===
Write-Host "[1/3] PostgreSQL..." -ForegroundColor Cyan
$pg = az containerapp show -n jir-postgres -g $rg 2>$null | ConvertFrom-Json
if (-not $pg) {
    Write-Host "  Creation en cours..." -ForegroundColor Yellow
    az containerapp create -n jir-postgres -g $rg --environment $env `
        --image postgres:16-alpine --target-port 5432 --ingress internal `
        --cpu 0.5 --memory 1Gi --min-replicas 1 --max-replicas 1 `
        --env-vars POSTGRES_USER=jir_user POSTGRES_PASSWORD=jir_password POSTGRES_DB=jir_db `
        --output none
    if ($LASTEXITCODE -eq 0) {
        Write-Host "  PostgreSQL deploye!" -ForegroundColor Green
    } else {
        Write-Host "  Erreur deploiement PostgreSQL" -ForegroundColor Red
        exit 1
    }
} else {
    Write-Host "  Deja deploye" -ForegroundColor Gray
}

$pgUrl = az containerapp show -n jir-postgres -g $rg --query "properties.configuration.ingress.fqdn" -o tsv
Write-Host "  URL: $pgUrl" -ForegroundColor White
Write-Host ""

# === API ===
Write-Host "[2/3] API..." -ForegroundColor Cyan
$api = az containerapp show -n jir-api -g $rg 2>$null | ConvertFrom-Json
if (-not $api) {
    Write-Host "  Creation en cours..." -ForegroundColor Yellow
    $connStr = "Host=$pgUrl;Port=5432;Database=jir_db;Username=jir_user;Password=jir_password"
    az containerapp create -n jir-api -g $rg --environment $env `
        --image $acrServer/jir-api:latest --target-port 8080 --ingress external `
        --cpu 0.5 --memory 1Gi --min-replicas 1 --max-replicas 3 `
        --registry-server $acrServer --registry-username $acrUser --registry-password $acrPass `
        --env-vars ASPNETCORE_ENVIRONMENT=Production ASPNETCORE_URLS=http://+:8080 "ConnectionStrings__DefaultConnection=$connStr" `
        --output none
    if ($LASTEXITCODE -eq 0) {
        Write-Host "  API deployee!" -ForegroundColor Green
    } else {
        Write-Host "  Erreur deploiement API" -ForegroundColor Red
        exit 1
    }
} else {
    Write-Host "  Deja deployee" -ForegroundColor Gray
}

$apiUrl = az containerapp show -n jir-api -g $rg --query "properties.configuration.ingress.fqdn" -o tsv
Write-Host "  URL: https://$apiUrl" -ForegroundColor White
Write-Host ""

# === BLAZOR ===
Write-Host "[3/3] Blazor..." -ForegroundColor Cyan
$blazor = az containerapp show -n jir-blazor -g $rg 2>$null | ConvertFrom-Json
if (-not $blazor) {
    Write-Host "  Creation en cours..." -ForegroundColor Yellow
    az containerapp create -n jir-blazor -g $rg --environment $env `
        --image $acrServer/jir-blazor:latest --target-port 8080 --ingress external `
        --cpu 0.5 --memory 1Gi --min-replicas 1 --max-replicas 3 `
        --registry-server $acrServer --registry-username $acrUser --registry-password $acrPass `
        --env-vars ASPNETCORE_ENVIRONMENT=Production ASPNETCORE_URLS=http://+:8080 "ApiBaseUrl=https://$apiUrl" `
        --output none
    if ($LASTEXITCODE -eq 0) {
        Write-Host "  Blazor deploye!" -ForegroundColor Green
    } else {
        Write-Host "  Erreur deploiement Blazor" -ForegroundColor Red
        exit 1
    }
} else {
    Write-Host "  Deja deploye" -ForegroundColor Gray
}

$blazorUrl = az containerapp show -n jir-blazor -g $rg --query "properties.configuration.ingress.fqdn" -o tsv
Write-Host "  URL: https://$blazorUrl" -ForegroundColor White
Write-Host ""

# === RESUME ===
Write-Host "======================================" -ForegroundColor Green
Write-Host "DEPLOIEMENT TERMINE!" -ForegroundColor Green
Write-Host "======================================" -ForegroundColor Green
Write-Host ""
Write-Host "Application deployee sur Azure Container Apps:" -ForegroundColor Cyan
Write-Host ""
Write-Host "  Blazor:     https://$blazorUrl" -ForegroundColor White
Write-Host "  API:        https://$apiUrl" -ForegroundColor White
Write-Host "  PostgreSQL: $pgUrl (interne)" -ForegroundColor Gray
Write-Host ""
Write-Host "Testez maintenant: https://$blazorUrl" -ForegroundColor Yellow
Write-Host ""
Write-Host "Commandes utiles:" -ForegroundColor Cyan
Write-Host "  Logs Blazor:  az containerapp logs show -n jir-blazor -g $rg --follow" -ForegroundColor Gray
Write-Host "  Logs API:     az containerapp logs show -n jir-api -g $rg --follow" -ForegroundColor Gray
Write-Host "  Portail:      https://portal.azure.com/#resource/subscriptions/02d95495-5cb9-4c60-acdd-07b696db2508/resourceGroups/$rg" -ForegroundColor Gray
Write-Host ""
