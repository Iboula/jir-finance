# Script de debug pour MouvementsStock
$baseUrl = "http://localhost:5263/api"

Write-Host "=== Debug MouvementsStock ===" -ForegroundColor Cyan

# Creer un magasin
Write-Host "`nCreation magasin..." -ForegroundColor Yellow
$magasinBody = @{
    code = "DEBUG"
    nom = "Debug Magasin"
    localisation = "Test"
} | ConvertTo-Json

try {
    $magasinId = Invoke-RestMethod -Uri "$baseUrl/magasins" -Method Post -Body $magasinBody -ContentType "application/json"
    Write-Host "Magasin ID: $magasinId" -ForegroundColor Green
} catch {
    Write-Host "Erreur: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "Details: $($_.ErrorDetails.Message)" -ForegroundColor Red
    exit
}

# Creer un article
Write-Host "`nCreation article..." -ForegroundColor Yellow
$articleBody = @{
    magasinId = $magasinId
    codeArticle = "DEBUG001"
    designation = "Article Debug"
    unite = "piece"
    seuilAlerte = 10
    prixUnitaire = 5.00
} | ConvertTo-Json

try {
    $articleId = Invoke-RestMethod -Uri "$baseUrl/articlesmagasin" -Method Post -Body $articleBody -ContentType "application/json"
    Write-Host "Article ID: $articleId" -ForegroundColor Green
} catch {
    Write-Host "Erreur: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "Details: $($_.ErrorDetails.Message)" -ForegroundColor Red
    exit
}

# Tenter de creer un mouvement
Write-Host "`nCreation mouvement ENTREE..." -ForegroundColor Yellow
$mouvementBody = @{
    articleId = $articleId
    typeMouvement = "Entrée"
    quantite = 50
    dateMouvement = (Get-Date).ToString("o")
    numeroDocument = "TEST001"
    motif = "Test debug"
} | ConvertTo-Json

Write-Host "Body envoyé:" -ForegroundColor Gray
Write-Host $mouvementBody -ForegroundColor Gray

try {
    $mouvementId = Invoke-RestMethod -Uri "$baseUrl/mouvementsstock" -Method Post -Body $mouvementBody -ContentType "application/json"
    Write-Host "Mouvement ID: $mouvementId" -ForegroundColor Green
} catch {
    Write-Host "Erreur: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "Details: $($_.ErrorDetails.Message)" -ForegroundColor Red
    Write-Host "Status Code: $($_.Exception.Response.StatusCode.value__)" -ForegroundColor Red
}
