# Deployment Bicep avec Azure CLI
$env:Path += ";C:\Program Files\Microsoft SDKs\Azure\CLI2\wbin"

Write-Host "Deploiement avec Bicep template..."
Write-Host ""

# Recuperer le mot de passe ACR
Write-Host "Recuperation credentials ACR..."
$acrPassword = az acr credential show --name rejirfinance --query "passwords[0].value" --output tsv

if (-not $acrPassword) {
    Write-Host "Erreur: Impossible de recuperer le mot de passe ACR" -ForegroundColor Red
    exit 1
}
Write-Host "Credentials ACR recuperes"
Write-Host ""

# Deployer avec Bicep
Write-Host "Deploiement de l'infrastructure (cela peut prendre 5-10 minutes)..."
Write-Host ""

az deployment group create `
    --resource-group rg-jir-finance `
    --template-file container-app.bicep `
    --parameters acrPassword=$acrPassword `
    --output json | ConvertFrom-Json | Out-Null

if ($LASTEXITCODE -eq 0) {
    Write-Host ""
    Write-Host "============================================================" -ForegroundColor Green
    Write-Host "Deploiement termine avec succes!" -ForegroundColor Green
    Write-Host "============================================================" -ForegroundColor Green
    Write-Host ""
    
    # Obtenir les URLs deployees
    $blazorUrl = az containerapp show --name jir-blazor --resource-group rg-jir-finance --query "properties.configuration.ingress.fqdn" --output tsv
    $apiUrl = az containerapp show --name jir-api --resource-group rg-jir-finance --query "properties.configuration.ingress.fqdn" --output tsv
    $postgresUrl = az containerapp show --name jir-postgres --resource-group rg-jir-finance --query "properties.configuration.ingress.fqdn" --output tsv
    
    Write-Host "URLs des applications:"
    Write-Host "  Blazor:     https://$blazorUrl" -ForegroundColor Cyan
    Write-Host "  API:        https://$apiUrl" -ForegroundColor Cyan
    Write-Host "  PostgreSQL: $postgresUrl (interne)" -ForegroundColor Gray
    Write-Host ""
    
    Write-Host "Pour tester:"
    Write-Host "  Ouvrez https://$blazorUrl dans votre navigateur" -ForegroundColor Yellow
    Write-Host ""
    
    Write-Host "Pour voir les logs:"
    Write-Host "  az containerapp logs show --name jir-blazor --resource-group rg-jir-finance --follow" -ForegroundColor Gray
    Write-Host "  az containerapp logs show --name jir-api --resource-group rg-jir-finance --follow" -ForegroundColor Gray
    Write-Host ""
} else {
    Write-Host ""
    Write-Host "Erreur lors du deploiement" -ForegroundColor Red
    Write-Host "Verifiez les logs ci-dessus pour plus de details" -ForegroundColor Red
    exit 1
}
