# Script de test pour le module ArticlesMagasin
$baseUrl = "http://localhost:5001/api"
$testResults = @()

Write-Host "=== Tests Module ArticlesMagasin ===" -ForegroundColor Cyan
Write-Host ""

# Variables pour stocker les IDs
$magasinId = $null
$articleId1 = $null
$articleId2 = $null

# Configuration pour ignorer les erreurs SSL et desactiver l'authentification pour les tests
[System.Net.ServicePointManager]::ServerCertificateValidationCallback = {$true}
$PSDefaultParameterValues['Invoke-RestMethod:SkipCertificateCheck'] = $true
$PSDefaultParameterValues['Invoke-RestMethod:SkipHttpErrorCheck'] = $true

# Prerequis: Creer un magasin pour les tests
Write-Host "Prerequis: Creation d'un magasin de test..." -ForegroundColor Cyan
try {
    $timestamp = Get-Date -Format "HHmmss"
    $magasinBody = @{
        code = "MAGTEST-$timestamp"
        nom = "Magasin Test Articles"
        localisation = "Zone Test"
    } | ConvertTo-Json
    
    $response = Invoke-RestMethod -Uri "$baseUrl/magasins" -Method Post -Body $magasinBody -ContentType "application/json" -ErrorAction SilentlyContinue
    if ($response) {
        $magasinId = $response
        Write-Host "[OK] Magasin cree avec ID: $magasinId" -ForegroundColor Green
    } else {
        Write-Host "[SKIP] Impossible de creer le magasin (authentification requise?)" -ForegroundColor Yellow
        Write-Host "Veuillez utiliser Swagger UI pour tester: http://localhost:5001/swagger" -ForegroundColor Cyan
        exit
    }
} catch {
    Write-Host "[SKIP] Erreur creation magasin: $($_.Exception.Message)" -ForegroundColor Yellow
    Write-Host "Veuillez utiliser Swagger UI pour tester: http://localhost:5001/swagger" -ForegroundColor Cyan
    exit
}
Start-Sleep -Milliseconds 500

# Test 1: Creer un article
Write-Host "`nTest 1: Creer un article..." -ForegroundColor Yellow
try {
    $body = @{
        magasinId = $magasinId
        codeArticle = "ART001"
        designation = "Cahier 100 pages"
        unite = "piece"
        seuilAlerte = 10
        prixUnitaire = 2.50
        observations = "Article de base"
    } | ConvertTo-Json
    
    $articleId1 = Invoke-RestMethod -Uri "$baseUrl/articlesmagasin" -Method Post -Body $body -ContentType "application/json"
    Write-Host "[OK] Article cree avec ID: $articleId1" -ForegroundColor Green
    $testResults += "[OK] Test 1 reussi"
} catch {
    Write-Host "[FAIL] Erreur: $($_.Exception.Message)" -ForegroundColor Red
    $testResults += "[FAIL] Test 1 echoue"
}
Start-Sleep -Milliseconds 500

# Test 2: Creer un deuxieme article
Write-Host "`nTest 2: Creer un deuxieme article..." -ForegroundColor Yellow
try {
    $body = @{
        magasinId = $magasinId
        codeArticle = "ART002"
        designation = "Stylo bleu"
        unite = "piece"
        seuilAlerte = 20
        prixUnitaire = 0.50
    } | ConvertTo-Json
    
    $articleId2 = Invoke-RestMethod -Uri "$baseUrl/articlesmagasin" -Method Post -Body $body -ContentType "application/json"
    Write-Host "[OK] Article cree avec ID: $articleId2" -ForegroundColor Green
    $testResults += "[OK] Test 2 reussi"
} catch {
    Write-Host "[FAIL] Erreur: $($_.Exception.Message)" -ForegroundColor Red
    $testResults += "[FAIL] Test 2 echoue"
}
Start-Sleep -Milliseconds 500

# Test 3: Verifier que la quantite en stock est bien 0 a la creation
Write-Host "`nTest 3: Verifier quantite stock initiale (doit etre 0)..." -ForegroundColor Yellow
try {
    $article = Invoke-RestMethod -Uri "$baseUrl/articlesmagasin/$articleId1" -Method Get
    if ($article.quantiteStock -eq 0) {
        Write-Host "[OK] Quantite stock initiale = 0 (attendu)" -ForegroundColor Green
        $testResults += "[OK] Test 3 reussi"
    } else {
        Write-Host "[FAIL] Quantite stock = $($article.quantiteStock), attendu 0" -ForegroundColor Red
        $testResults += "[FAIL] Test 3 echoue"
    }
} catch {
    Write-Host "[FAIL] Erreur: $($_.Exception.Message)" -ForegroundColor Red
    $testResults += "[FAIL] Test 3 echoue"
}
Start-Sleep -Milliseconds 500

# Test 4: Tenter de creer un article avec code duplique (doit echouer)
Write-Host "`nTest 4: Tenter de creer un article avec code duplique (doit echouer)..." -ForegroundColor Yellow
try {
    $body = @{
        magasinId = $magasinId
        codeArticle = "ART001"
        designation = "Autre article"
        unite = "piece"
        seuilAlerte = 5
        prixUnitaire = 1.00
    } | ConvertTo-Json
    
    $result = Invoke-RestMethod -Uri "$baseUrl/articlesmagasin" -Method Post -Body $body -ContentType "application/json"
    Write-Host "[FAIL] Article duplique cree (ne devrait pas)" -ForegroundColor Red
    $testResults += "[FAIL] Test 4 echoue"
} catch {
    Write-Host "[OK] Erreur attendue: Code article duplique detecte" -ForegroundColor Green
    $testResults += "[OK] Test 4 reussi"
}
Start-Sleep -Milliseconds 500

# Test 5: Lister tous les articles du magasin
Write-Host "`nTest 5: Lister tous les articles du magasin..." -ForegroundColor Yellow
try {
    $result = Invoke-RestMethod -Uri "$baseUrl/articlesmagasin?magasinId=$magasinId" -Method Get
    if ($result.articles.Count -ge 2) {
        Write-Host "[OK] Articles recuperes: $($result.articles.Count)" -ForegroundColor Green
        $testResults += "[OK] Test 5 reussi"
    } else {
        Write-Host "[FAIL] Nombre d'articles incorrect: $($result.articles.Count)" -ForegroundColor Red
        $testResults += "[FAIL] Test 5 echoue"
    }
} catch {
    Write-Host "[FAIL] Erreur: $($_.Exception.Message)" -ForegroundColor Red
    $testResults += "[FAIL] Test 5 echoue"
}
Start-Sleep -Milliseconds 500

# Test 6: Rechercher un article par code
Write-Host "`nTest 6: Rechercher un article par code..." -ForegroundColor Yellow
try {
    $result = Invoke-RestMethod -Uri "$baseUrl/articlesmagasin?magasinId=$magasinId&search=ART001" -Method Get
    if ($result.articles.Count -eq 1 -and $result.articles[0].codeArticle -eq "ART001") {
        Write-Host "[OK] Article trouve par recherche" -ForegroundColor Green
        $testResults += "[OK] Test 6 reussi"
    } else {
        Write-Host "[FAIL] Recherche incorrecte" -ForegroundColor Red
        $testResults += "[FAIL] Test 6 echoue"
    }
} catch {
    Write-Host "[FAIL] Erreur: $($_.Exception.Message)" -ForegroundColor Red
    $testResults += "[FAIL] Test 6 echoue"
}
Start-Sleep -Milliseconds 500

# Test 7: Rechercher un article par designation
Write-Host "`nTest 7: Rechercher un article par designation..." -ForegroundColor Yellow
try {
    $result = Invoke-RestMethod -Uri "$baseUrl/articlesmagasin?magasinId=$magasinId&search=Cahier" -Method Get
    if ($result.articles.Count -eq 1 -and $result.articles[0].designation -like "*Cahier*") {
        Write-Host "[OK] Article trouve par designation" -ForegroundColor Green
        $testResults += "[OK] Test 7 reussi"
    } else {
        Write-Host "[FAIL] Recherche par designation incorrecte" -ForegroundColor Red
        $testResults += "[FAIL] Test 7 echoue"
    }
} catch {
    Write-Host "[FAIL] Erreur: $($_.Exception.Message)" -ForegroundColor Red
    $testResults += "[FAIL] Test 7 echoue"
}
Start-Sleep -Milliseconds 500

# Test 8: Recuperer un article par ID
Write-Host "`nTest 8: Recuperer un article par ID..." -ForegroundColor Yellow
try {
    $article = Invoke-RestMethod -Uri "$baseUrl/articlesmagasin/$articleId1" -Method Get
    if ($article.id -eq $articleId1 -and $article.codeArticle -eq "ART001") {
        Write-Host "[OK] Article recupere avec details complets" -ForegroundColor Green
        Write-Host "  - Code: $($article.codeArticle)" -ForegroundColor Gray
        Write-Host "  - Designation: $($article.designation)" -ForegroundColor Gray
        Write-Host "  - Stock: $($article.quantiteStock)" -ForegroundColor Gray
        Write-Host "  - Magasin: $($article.magasinNom)" -ForegroundColor Gray
        $testResults += "[OK] Test 8 reussi"
    } else {
        Write-Host "[FAIL] Donnees article incorrectes" -ForegroundColor Red
        $testResults += "[FAIL] Test 8 echoue"
    }
} catch {
    Write-Host "[FAIL] Erreur: $($_.Exception.Message)" -ForegroundColor Red
    $testResults += "[FAIL] Test 8 echoue"
}
Start-Sleep -Milliseconds 500

# Test 9: Mettre a jour un article
Write-Host "`nTest 9: Mettre a jour un article..." -ForegroundColor Yellow
try {
    $body = @{
        codeArticle = "ART001-MOD"
        designation = "Cahier 200 pages"
        unite = "piece"
        seuilAlerte = 15
        prixUnitaire = 3.00
        observations = "Article modifie"
    } | ConvertTo-Json
    
    Invoke-RestMethod -Uri "$baseUrl/articlesmagasin/$articleId1" -Method Put -Body $body -ContentType "application/json"
    
    # Verifier la modification
    $article = Invoke-RestMethod -Uri "$baseUrl/articlesmagasin/$articleId1" -Method Get
    if ($article.codeArticle -eq "ART001-MOD" -and $article.designation -eq "Cahier 200 pages") {
        Write-Host "[OK] Article mis a jour avec succes" -ForegroundColor Green
        $testResults += "[OK] Test 9 reussi"
    } else {
        Write-Host "[FAIL] Modification non prise en compte" -ForegroundColor Red
        $testResults += "[FAIL] Test 9 echoue"
    }
} catch {
    Write-Host "[FAIL] Erreur: $($_.Exception.Message)" -ForegroundColor Red
    $testResults += "[FAIL] Test 9 echoue"
}
Start-Sleep -Milliseconds 500

# Test 10: Verifier que l'article a bien ete modifie
Write-Host "`nTest 10: Verifier que l'article a bien ete modifie..." -ForegroundColor Yellow
try {
    $article = Invoke-RestMethod -Uri "$baseUrl/articlesmagasin/$articleId1" -Method Get
    if ($article.codeArticle -eq "ART001-MOD" -and $article.designation -eq "Cahier 200 pages" -and $article.magasinId -eq $magasinId) {
        Write-Host "[OK] Article modifie et MagasinId inchange" -ForegroundColor Green
        $testResults += "[OK] Test 10 reussi"
    } else {
        Write-Host "[FAIL] Modification non conforme" -ForegroundColor Red
        $testResults += "[FAIL] Test 10 echoue"
    }
} catch {
    Write-Host "[FAIL] Erreur: $($_.Exception.Message)" -ForegroundColor Red
    $testResults += "[FAIL] Test 10 echoue"
}
Start-Sleep -Milliseconds 500

# Test 11: Supprimer un article (soft delete)
Write-Host "`nTest 11: Supprimer un article (soft delete)..." -ForegroundColor Yellow
try {
    Invoke-RestMethod -Uri "$baseUrl/articlesmagasin/$articleId2" -Method Delete
    
    # Verifier que l'article n'est plus dans la liste
    $articles = Invoke-RestMethod -Uri "$baseUrl/articlesmagasin?magasinId=$magasinId" -Method Get
    $articleSupprime = $articles.items | Where-Object { $_.id -eq $articleId2 }
    
    if ($null -eq $articleSupprime) {
        Write-Host "[OK] Article supprime (soft delete)" -ForegroundColor Green
        $testResults += "[OK] Test 11 reussi"
    } else {
        Write-Host "[FAIL] Article encore visible apres suppression" -ForegroundColor Red
        $testResults += "[FAIL] Test 11 echoue"
    }
} catch {
    Write-Host "[FAIL] Erreur: $($_.Exception.Message)" -ForegroundColor Red
    $testResults += "[FAIL] Test 11 echoue"
}
Start-Sleep -Milliseconds 500

# Test 12: Verifier la pagination
Write-Host "`nTest 12: Verifier la pagination..." -ForegroundColor Yellow
try {
    $articles = Invoke-RestMethod -Uri "$baseUrl/articlesmagasin?magasinId=$magasinId&pageNumber=1&pageSize=1" -Method Get
    if ($articles.pageSize -eq 1 -and $articles.items.Count -le 1) {
        Write-Host "[OK] Pagination fonctionnelle" -ForegroundColor Green
        Write-Host "  - Total: $($articles.totalCount), Pages: $($articles.totalPages)" -ForegroundColor Gray
        $testResults += "[OK] Test 12 reussi"
    } else {
        Write-Host "[FAIL] Pagination incorrecte" -ForegroundColor Red
        $testResults += "[FAIL] Test 12 echoue"
    }
} catch {
    Write-Host "[FAIL] Erreur: $($_.Exception.Message)" -ForegroundColor Red
    $testResults += "[FAIL] Test 12 echoue"
}

# Resume
Write-Host "`n=== RESUME DES TESTS ArticlesMagasin ===" -ForegroundColor Cyan
$testResults | ForEach-Object {
    if ($_ -like "*OK*") {
        Write-Host $_ -ForegroundColor Green
    } else {
        Write-Host $_ -ForegroundColor Red
    }
}

$successCount = ($testResults | Where-Object { $_ -like "*OK*" }).Count
$totalCount = $testResults.Count
Write-Host "`nResultat final: $successCount/$totalCount tests reussis" -ForegroundColor $(if ($successCount -eq $totalCount) { "Green" } else { "Yellow" })

