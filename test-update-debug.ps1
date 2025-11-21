# Test debug Update ArticleMagasin
$baseUrl = "http://localhost:5001/api"

Write-Host "=== Test Update ArticleMagasin ===" -ForegroundColor Cyan

# 1. Creer Magasin
$magasinBody = @{
    code = "MAG-TEST-UPDATE"
    nom = "Magasin Test"
    localisation = "Test"
} | ConvertTo-Json

$magasin = Invoke-RestMethod -Uri "$baseUrl/magasins" -Method Post -Body $magasinBody -ContentType "application/json"
Write-Host "[OK] Magasin cree: $($magasin)" -ForegroundColor Green
$magasinId = $magasin

# 2. Creer Article
$articleBody = @{
    magasinId = $magasinId
    codeArticle = "ART-UPDATE"
    designation = "Article Test Update"
    unite = "piece"
    seuilAlerte = 10
    prixUnitaire = 5.00
} | ConvertTo-Json

$article = Invoke-RestMethod -Uri "$baseUrl/articlesmagasin" -Method Post -Body $articleBody -ContentType "application/json"
Write-Host "[OK] Article cree: $($article)" -ForegroundColor Green
$articleId = $article

# 3. Update Article
Write-Host "`nTest UPDATE..." -ForegroundColor Yellow
try {
    $updateBody = @{
        codeArticle = "ART-UPDATE-MOD"
        designation = "Article Modifie"
        unite = "piece"
        seuilAlerte = 15
        prixUnitaire = 7.50
        observations = "Modifie"
    } | ConvertTo-Json
    
    Write-Host "Body: $updateBody" -ForegroundColor Gray
    Write-Host "URL: $baseUrl/articlesmagasin/$articleId" -ForegroundColor Gray
    
    $result = Invoke-RestMethod -Uri "$baseUrl/articlesmagasin/$articleId" -Method Put -Body $updateBody -ContentType "application/json"
    Write-Host "[OK] Article mis a jour" -ForegroundColor Green
} catch {
    Write-Host "[FAIL] Erreur: $($_.Exception.Message)" -ForegroundColor Red
    if ($_.ErrorDetails.Message) {
        Write-Host "Details: $($_.ErrorDetails.Message)" -ForegroundColor Red
    }
}
