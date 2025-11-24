# Verification et completion du deployment
$env:Path = [System.Environment]::GetEnvironmentVariable("Path","Machine") + ";" + [System.Environment]::GetEnvironmentVariable("Path","User")

Write-Host "Verification de l'etat du deploiement..." -ForegroundColor Cyan
Write-Host ""

# Verifier les ressources existantes
Write-Host "Ressources dans rg-jir-finance:" -ForegroundColor Yellow
az resource list -g rg-jir-finance -o table

Write-Host ""
Write-Host "Environnements Container Apps:" -ForegroundColor Yellow
$envs = az containerapp env list -g rg-jir-finance 2>$null | ConvertFrom-Json

if ($envs -and $envs.Count -gt 0) {
    Write-Host "Environnement trouve: $($envs[0].name) - Etat: $($envs[0].properties.provisioningState)" -ForegroundColor Green
    $envName = $envs[0].name
    
    # Continuer avec le deploiement des apps
    Write-Host ""
    Write-Host "Deploiement des applications..." -ForegroundColor Cyan
    
    # Credentials ACR
    $acrUser = az acr credential show --name rejirfinance --query "username" -o tsv
    $acrPass = az acr credential show --name rejirfinance --query "passwords[0].value" -o tsv
    
    # PostgreSQL
    Write-Host "1. PostgreSQL..."
    $pgExists = az containerapp show -n jir-postgres -g rg-jir-finance 2>$null
    if (-not $pgExists) {
        az containerapp create -n jir-postgres -g rg-jir-finance --environment $envName `
            --image postgres:16-alpine --target-port 5432 --ingress internal `
            --cpu 0.5 --memory 1Gi --min-replicas 1 --max-replicas 1 `
            --env-vars POSTGRES_USER=jir_user POSTGRES_PASSWORD=jir_password POSTGRES_DB=jir_db
    }
    $pgUrl = az containerapp show -n jir-postgres -g rg-jir-finance --query "properties.configuration.ingress.fqdn" -o tsv
    Write-Host "   PostgreSQL: $pgUrl" -ForegroundColor Green
    
    # API
    Write-Host "2. API..."
    $apiExists = az containerapp show -n jir-api -g rg-jir-finance 2>$null
    if (-not $apiExists) {
        $connStr = "Host=$pgUrl;Port=5432;Database=jir_db;Username=jir_user;Password=jir_password"
        az containerapp create -n jir-api -g rg-jir-finance --environment $envName `
            --image rejirfinance.azurecr.io/jir-api:latest --target-port 8080 --ingress external `
            --cpu 0.5 --memory 1Gi --min-replicas 1 --max-replicas 3 `
            --registry-server rejirfinance.azurecr.io --registry-username $acrUser --registry-password $acrPass `
            --env-vars ASPNETCORE_ENVIRONMENT=Production ASPNETCORE_URLS=http://+:8080 "ConnectionStrings__DefaultConnection=$connStr"
    }
    $apiUrl = az containerapp show -n jir-api -g rg-jir-finance --query "properties.configuration.ingress.fqdn" -o tsv
    Write-Host "   API: https://$apiUrl" -ForegroundColor Green
    
    # Blazor
    Write-Host "3. Blazor..."
    $blazorExists = az containerapp show -n jir-blazor -g rg-jir-finance 2>$null
    if (-not $blazorExists) {
        az containerapp create -n jir-blazor -g rg-jir-finance --environment $envName `
            --image rejirfinance.azurecr.io/jir-blazor:latest --target-port 8080 --ingress external `
            --cpu 0.5 --memory 1Gi --min-replicas 1 --max-replicas 3 `
            --registry-server rejirfinance.azurecr.io --registry-username $acrUser --registry-password $acrPass `
            --env-vars ASPNETCORE_ENVIRONMENT=Production ASPNETCORE_URLS=http://+:8080 "ApiBaseUrl=https://$apiUrl"
    }
    $blazorUrl = az containerapp show -n jir-blazor -g rg-jir-finance --query "properties.configuration.ingress.fqdn" -o tsv
    Write-Host "   Blazor: https://$blazorUrl" -ForegroundColor Green
    
    Write-Host ""
    Write-Host "DEPLOIEMENT TERMINE!" -ForegroundColor Green
    Write-Host "Testez: https://$blazorUrl" -ForegroundColor Cyan
} else {
    Write-Host "Aucun environnement trouve. Creation en cours..." -ForegroundColor Yellow
    Write-Host "L'environnement Container Apps est en cours de creation."
    Write-Host "Cette operation prend 3-5 minutes."
    Write-Host ""
    Write-Host "Options:" -ForegroundColor Cyan
    Write-Host "1. Attendre et relancer ce script dans quelques minutes"
    Write-Host "2. Creer manuellement l'environnement avec:"
    Write-Host "   az containerapp env create --name jir-finance-env --resource-group rg-jir-finance --location canadacentral"
}
