$baseUrl = "http://localhost:5001/api"

Write-Host "=== Test rapide MouvementsStock ===" -ForegroundColor Cyan

# 1. Creer Magasin
Write-Host "`nCreation magasin..." -ForegroundColor Yellow
try {
    $magasinBody = @{
        code = "MAG-TEST-DEBUG"
        nom = "Magasin Debug"
        localisation = "Test"
    } | ConvertTo-Json
    
    $magasin = Invoke-RestMethod -Uri "$baseUrl/magasins" -Method Post -Body $magasinBody -ContentType "application/json"
    Write-Host "[OK] Magasin cree: $($magasin.id)" -ForegroundColor Green
    $magasinId = $magasin.id
} catch {
    Write-Host "[FAIL] Erreur creation magasin: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "Response: $($_.ErrorDetails.Message)" -ForegroundColor Red
    exit
}

# 2. Creer Article
Write-Host "`nCreation article..." -ForegroundColor Yellow
try {
    $articleBody = @{
        magasinId = $magasinId
        codeArticle = "ART-DEBUG"
        designation = "Article Debug"
        unite = "piece"
        seuilAlerte = 10
        prixUnitaire = 5.00
    } | ConvertTo-Json
    
    $article = Invoke-RestMethod -Uri "$baseUrl/articlesmagasin" -Method Post -Body $articleBody -ContentType "application/json"
    Write-Host "[OK] Article cree: $($article.id)" -ForegroundColor Green
    $articleId = $article.id
} catch {
    Write-Host "[FAIL] Erreur creation article: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "Response: $($_.ErrorDetails.Message)" -ForegroundColor Red
    exit
}

# 3. Creer mouvement ENTREE
Write-Host "`nCreation mouvement ENTREE..." -ForegroundColor Yellow
try {
    $mouvBody = @{
        articleId = $articleId
        typeMouvement = "Entree"
        quantite = 50
        dateMouvement = (Get-Date).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ")
        motif = "Test Entree"
    } | ConvertTo-Json
    
    Write-Host "Body JSON: $mouvBody" -ForegroundColor Gray
    
    $mouvement = Invoke-RestMethod -Uri "$baseUrl/mouvementsstock" -Method Post -Body $mouvBody -ContentType "application/json"
    Write-Host "[OK] Mouvement cree: $($mouvement)" -ForegroundColor Green
} catch {
    Write-Host "[FAIL] Erreur creation mouvement: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "Response: $($_.ErrorDetails.Message)" -ForegroundColor Red
    if ($_.Exception.Response) {
        $reader = New-Object System.IO.StreamReader($_.Exception.Response.GetResponseStream())
        $reader.BaseStream.Position = 0
        $reader.DiscardBufferedData()
        $responseBody = $reader.ReadToEnd()
        Write-Host "Full Response Body: $responseBody" -ForegroundColor Red
    }
}

Write-Host "`n=== Test termine ===" -ForegroundColor Cyan
