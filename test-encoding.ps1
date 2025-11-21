# Test simple pour capturer l'erreur exacte
$ErrorActionPreference = "Continue"

# Démarrer l'API
Write-Host "Démarrage de l'API..." -ForegroundColor Cyan
$apiProcess = Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd C:\Users\iboul\OneDrive\Documents\PROJETS\MOPRO\JIR; dotnet run --project src\JIR.WebApi\JIR.WebApi.csproj" -PassThru

Start-Sleep -Seconds 15

# Test 1: Créer un magasin (devrait marcher)
Write-Host "`nTest Magasin..." -ForegroundColor Yellow
try {
    $body = '{"code":"TEST","nom":"Test","localisation":"Test"}'
    $result = Invoke-WebRequest -Uri "http://localhost:5263/api/magasins" -Method Post -Body $body -ContentType "application/json" -UseBasicParsing
    Write-Host "Status: $($result.StatusCode)" -ForegroundColor Green
    $magasinId = ($result.Content | ConvertFrom-Json)
    Write-Host "Magasin ID: $magasinId" -ForegroundColor Green
    
    # Test 2: Créer un article
    Write-Host "`nTest Article..." -ForegroundColor Yellow
    $articleBody = @{
        magasinId = $magasinId
        codeArticle = "TEST001"
        designation = "Test"
        unite = "piece"
        seuilAlerte = 10
        prixUnitaire = 5.00
    } | ConvertTo-Json -Compress
    
    $result2 = Invoke-WebRequest -Uri "http://localhost:5263/api/articlesmagasin" -Method Post -Body $articleBody -ContentType "application/json; charset=utf-8" -UseBasicParsing
    Write-Host "Status: $($result2.StatusCode)" -ForegroundColor Green
    $articleId = ($result2.Content | ConvertFrom-Json)
    Write-Host "Article ID: $articleId" -ForegroundColor Green
    
    # Test 3: Créer un mouvement avec Entree (sans accent)
    Write-Host "`nTest Mouvement (Entree sans accent)..." -ForegroundColor Yellow
    $mouvementBody = @{
        articleId = $articleId
        typeMouvement = "Entree"
        quantite = 50
        dateMouvement = (Get-Date).ToString("yyyy-MM-ddTHH:mm:ssZ")
        numeroDocument = "TEST"
        motif = "Test"
    } | ConvertTo-Json -Compress
    
    Write-Host "Body: $mouvementBody" -ForegroundColor Gray
    
    try {
        $result3 = Invoke-WebRequest -Uri "http://localhost:5263/api/mouvementsstock" -Method Post -Body $mouvementBody -ContentType "application/json; charset=utf-8" -UseBasicParsing
        Write-Host "Status: $($result3.StatusCode)" -ForegroundColor Green
    } catch {
        Write-Host "Erreur: $($_.Exception.Message)" -ForegroundColor Red
        if ($_.Exception.Response) {
            $reader = New-Object System.IO.StreamReader($_.Exception.Response.GetResponseStream())
            $responseBody = $reader.ReadToEnd()
            Write-Host "Response: $responseBody" -ForegroundColor Red
        }
    }
    
    # Test 4: Créer un mouvement avec Entrée (avec accent UTF-8)
    Write-Host "`nTest Mouvement (Entrée avec accent)..." -ForegroundColor Yellow
    $mouvementBody2 = @{
        articleId = $articleId
        typeMouvement = "Entrée"
        quantite = 50
        dateMouvement = (Get-Date).ToString("yyyy-MM-ddTHH:mm:ssZ")
        numeroDocument = "TEST2"
        motif = "Test2"
    } | ConvertTo-Json -Compress
    
    Write-Host "Body: $mouvementBody2" -ForegroundColor Gray
    
    try {
        $result4 = Invoke-WebRequest -Uri "http://localhost:5263/api/mouvementsstock" -Method Post -Body $mouvementBody2 -ContentType "application/json; charset=utf-8" -UseBasicParsing
        Write-Host "Status: $($result4.StatusCode)" -ForegroundColor Green
    } catch {
        Write-Host "Erreur: $($_.Exception.Message)" -ForegroundColor Red
        if ($_.Exception.Response) {
            $reader = New-Object System.IO.StreamReader($_.Exception.Response.GetResponseStream())
            $responseBody = $reader.ReadToEnd()
            Write-Host "Response: $responseBody" -ForegroundColor Red
        }
    }
    
} catch {
    Write-Host "Erreur: $($_.Exception.Message)" -ForegroundColor Red
    if ($_.Exception.Response) {
        $reader = New-Object System.IO.StreamReader($_.Exception.Response.GetResponseStream())
        $responseBody = $reader.ReadToEnd()
        Write-Host "Response: $responseBody" -ForegroundColor Red
    }
}

Write-Host "`nAppuyez sur une touche pour arrêter..." -ForegroundColor Cyan
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")

# Arrêter l'API
if ($apiProcess) {
    Stop-Process -Id $apiProcess.Id -Force
}
