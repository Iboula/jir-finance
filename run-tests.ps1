# Script pour demarrer l'API et executer les tests automatiquement
$apiProcess = $null

Write-Host "=== Demarrage de l'API ===" -ForegroundColor Cyan

# Demarrer l'API en arriere-plan
$apiProcess = Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd C:\Users\iboul\OneDrive\Documents\PROJETS\MOPRO\JIR; dotnet run --project src\JIR.WebApi\JIR.WebApi.csproj --urls='http://localhost:5001'" -PassThru

# Attendre que l'API soit prete
Write-Host "Attente du demarrage de l'API..." -ForegroundColor Yellow
Start-Sleep -Seconds 15

# Verifier que l'API repond
$apiReady = $false
$maxAttempts = 10
$attempt = 0

while (-not $apiReady -and $attempt -lt $maxAttempts) {
    try {
        $response = Invoke-WebRequest -Uri "http://localhost:5001/swagger/index.html" -UseBasicParsing -TimeoutSec 2
        if ($response.StatusCode -eq 200) {
            $apiReady = $true
            Write-Host "[OK] API prete!" -ForegroundColor Green
        }
    } catch {
        $attempt++
        Write-Host "Tentative $attempt/$maxAttempts..." -ForegroundColor Gray
        Start-Sleep -Seconds 2
    }
}

if (-not $apiReady) {
    Write-Host "[FAIL] L'API n'a pas demarre correctement" -ForegroundColor Red
    if ($apiProcess) {
        Stop-Process -Id $apiProcess.Id -Force
    }
    exit 1
}

Write-Host ""
Write-Host "=== Execution des tests ArticlesMagasin ===" -ForegroundColor Cyan
& "C:\Users\iboul\OneDrive\Documents\PROJETS\MOPRO\JIR\test-articlesmagasin.ps1"

Write-Host ""
Write-Host "=== Execution des tests MouvementsStock ===" -ForegroundColor Cyan
& "C:\Users\iboul\OneDrive\Documents\PROJETS\MOPRO\JIR\test-mouvementsstock.ps1"

Write-Host ""
Write-Host "=== Tests termines ===" -ForegroundColor Cyan
Write-Host "Appuyez sur une touche pour arreter l'API..."
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")

# Arreter l'API
if ($apiProcess) {
    Write-Host "Arret de l'API..." -ForegroundColor Yellow
    Stop-Process -Id $apiProcess.Id -Force
    Write-Host "API arretee." -ForegroundColor Green
}
