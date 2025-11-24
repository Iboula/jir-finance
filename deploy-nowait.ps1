# Deploiement rapide avec --no-wait
$env:Path = [System.Environment]::GetEnvironmentVariable("Path","Machine") + ";" + [System.Environment]::GetEnvironmentVariable("Path","User")

Write-Host "Deploiement API et Blazor..." -ForegroundColor Cyan
Write-Host ""

# Variables
$rg = "rg-jir-finance"
$env = "jir-finance-env"
$acrServer = "rejirfinance.azurecr.io"

# Credentials
$acrUser = az acr credential show --name rejirfinance --query "username" -o tsv
$acrPass = az acr credential show --name rejirfinance --query "passwords[0].value" -o tsv
$pgUrl = "jir-postgres.internal.blackfield-a8167a21.canadacentral.azurecontainerapps.io"

# API
Write-Host "Deploiement API (en arriere-plan)..."
$connStr = "Host=$pgUrl;Port=5432;Database=jir_db;Username=jir_user;Password=jir_password"
az containerapp create -n jir-api -g $rg --environment $env `
    --image $acrServer/jir-api:latest --target-port 8080 --ingress external `
    --cpu 0.5 --memory 1Gi --min-replicas 1 --max-replicas 3 `
    --registry-server $acrServer --registry-username $acrUser --registry-password $acrPass `
    --env-vars ASPNETCORE_ENVIRONMENT=Production ASPNETCORE_URLS=http://+:8080 "ConnectionStrings__DefaultConnection=$connStr" `
    --no-wait

Write-Host "API lance"
Write-Host ""

# Blazor (apres avoir l'URL API)
Write-Host "Attente 30s avant Blazor..."
Start-Sleep -Seconds 30

Write-Host "Deploiement Blazor (en arriere-plan)..."
$apiUrl = "jir-api.blackfield-a8167a21.canadacentral.azurecontainerapps.io"
az containerapp create -n jir-blazor -g $rg --environment $env `
    --image $acrServer/jir-blazor:latest --target-port 8080 --ingress external `
    --cpu 0.5 --memory 1Gi --min-replicas 1 --max-replicas 3 `
    --registry-server $acrServer --registry-username $acrUser --registry-password $acrPass `
    --env-vars ASPNETCORE_ENVIRONMENT=Production ASPNETCORE_URLS=http://+:8080 "ApiBaseUrl=https://$apiUrl" `
    --no-wait

Write-Host "Blazor lance"
Write-Host ""
Write-Host "Deploiements lances en arriere-plan!" -ForegroundColor Green
Write-Host "Attente 2-3 minutes pour la finalisation..." -ForegroundColor Yellow
Write-Host ""
Write-Host "Verifiez l'etat avec:"
Write-Host "  az containerapp list -g $rg -o table" -ForegroundColor Gray
