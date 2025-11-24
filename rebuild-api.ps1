# Script pour reconstruire l'API dans Azure depuis GitHub
$env:Path = [System.Environment]::GetEnvironmentVariable("Path","Machine") + ";" + [System.Environment]::GetEnvironmentVariable("Path","User")

$rg = "rg-jir-finance"
$acrName = "rejirfinance"

Write-Host "=== Reconstruction de l'image API ===" -ForegroundColor Cyan
Write-Host ""

# Option 1: Utiliser ACR Tasks pour build depuis GitHub
Write-Host "Option 1: Build avec ACR Tasks depuis GitHub..." -ForegroundColor Yellow
Write-Host ""

$repoUrl = "https://github.com/Iboula/jir-finance.git"
$branch = "feature/docker-charts"
$dockerfile = "src/JIR.WebAPI/Dockerfile"

Write-Host "Construction de l'image dans ACR..."
Write-Host "  Repository: $repoUrl"
Write-Host "  Branch: $branch"
Write-Host "  Dockerfile: $dockerfile"
Write-Host ""

az acr build `
    --registry $acrName `
    --image jir-api:latest `
    --file $dockerfile `
    --platform linux `
    $repoUrl#${branch}

if ($LASTEXITCODE -eq 0) {
    Write-Host ""
    Write-Host "Image construite avec succes!" -ForegroundColor Green
    Write-Host ""
    Write-Host "Mise a jour du Container App..."
    
    az containerapp update `
        --name jir-api `
        --resource-group $rg `
        --image rejirfinance.azurecr.io/jir-api:latest
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host ""
        Write-Host "Container App mis a jour!" -ForegroundColor Green
        Write-Host ""
        Write-Host "Verification..."
        Start-Sleep -Seconds 30
        
        az containerapp show `
            --name jir-api `
            --resource-group $rg `
            --query "{Name:name, Status:properties.provisioningState, URL:properties.configuration.ingress.fqdn, Replicas:properties.runningStatus}" `
            --output table
    }
} else {
    Write-Host ""
    Write-Host "Erreur lors de la construction" -ForegroundColor Red
    Write-Host ""
    Write-Host "Alternative: Attendez que Docker Desktop redémarre et utilisez:"
    Write-Host "  docker build -t jir-api:latest -f src/JIR.WebAPI/Dockerfile ." -ForegroundColor Gray
    Write-Host "  docker tag jir-api:latest rejirfinance.azurecr.io/jir-api:latest" -ForegroundColor Gray
    Write-Host "  docker push rejirfinance.azurecr.io/jir-api:latest" -ForegroundColor Gray
}
