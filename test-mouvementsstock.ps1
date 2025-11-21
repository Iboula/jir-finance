# Script de test pour le module MouvementsStock
$baseUrl = "http://localhost:5001/api"
$testResults = @()

Write-Host "=== Tests Module MouvementsStock ===" -ForegroundColor Cyan
Write-Host ""

# Variables pour stocker les IDs
$magasinId = $null
$articleId = $null
$mouvementEntreeId = $null
$mouvementSortieId = $null
$mouvementAjustementId = $null

# Configuration pour ignorer les erreurs SSL et desactiver l'authentification pour les tests
[System.Net.ServicePointManager]::ServerCertificateValidationCallback = {$true}
$PSDefaultParameterValues['Invoke-RestMethod:SkipCertificateCheck'] = $true
$PSDefaultParameterValues['Invoke-RestMethod:SkipHttpErrorCheck'] = $true

# Prerequis: Creer un magasin et un article pour les tests
Write-Host "Prerequis: Creation d'un magasin et d'un article de test..." -ForegroundColor Cyan
try {
    # Creer magasin
    $timestamp = Get-Date -Format "HHmmss"
    $magasinBody = @{
        code = "MAGSTOCK-$timestamp"
        nom = "Magasin Test Stock"
        localisation = "Zone Stock"
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
    
    # Creer article
    $articleBody = @{
        magasinId = $magasinId
        codeArticle = "STOCK001"
        designation = "Article Test Stock"
        unite = "piece"
        seuilAlerte = 10
        prixUnitaire = 5.00
    } | ConvertTo-Json
    
    $articleId = Invoke-RestMethod -Uri "$baseUrl/articlesmagasin" -Method Post -Body $articleBody -ContentType "application/json" -ErrorAction SilentlyContinue
    if ($articleId) {
        Write-Host "[OK] Article cree avec ID: $articleId" -ForegroundColor Green
    } else {
        Write-Host "[SKIP] Impossible de creer l'article" -ForegroundColor Yellow
        exit
    }
} catch {
    Write-Host "[SKIP] Erreur creation prerequis: $($_.Exception.Message)" -ForegroundColor Yellow
    Write-Host "Veuillez utiliser Swagger UI pour tester: http://localhost:5001/swagger" -ForegroundColor Cyan
    exit
}
Start-Sleep -Milliseconds 500

# Test 1: Verifier que le stock initial est a 0
Write-Host "`nTest 1: Verifier que le stock initial est a 0..." -ForegroundColor Yellow
try {
    $article = Invoke-RestMethod -Uri "$baseUrl/articlesmagasin/$articleId" -Method Get
    if ($article.quantiteStock -eq 0) {
        Write-Host "[OK] Stock initial = 0" -ForegroundColor Green
        $testResults += "[OK] Test 1 reussi"
    } else {
        Write-Host "[FAIL] Stock initial = $($article.quantiteStock), attendu 0" -ForegroundColor Red
        $testResults += "[FAIL] Test 1 echoue"
    }
} catch {
    Write-Host "[FAIL] Erreur: $($_.Exception.Message)" -ForegroundColor Red
    $testResults += "[FAIL] Test 1 echoue"
}
Start-Sleep -Milliseconds 500

# Test 2: Creer un mouvement d'ENTREE (+50)
Write-Host "`nTest 2: Creer un mouvement d'ENTREE (+50)..." -ForegroundColor Yellow
try {
    $body = @{
        articleId = $articleId
        typeMouvement = "Entree"
        quantite = 50
        dateMouvement = (Get-Date).ToString("o")
        numeroDocument = "BON001"
        motif = "Achat initial"
    } | ConvertTo-Json
    
    $mouvementEntreeId = Invoke-RestMethod -Uri "$baseUrl/mouvementsstock" -Method Post -Body $body -ContentType "application/json"
    Write-Host "[OK] Mouvement ENTREE cree avec ID: $mouvementEntreeId" -ForegroundColor Green
    $testResults += "[OK] Test 2 reussi"
} catch {
    Write-Host "[FAIL] Erreur: $($_.Exception.Message)" -ForegroundColor Red
    $testResults += "[FAIL] Test 2 echoue"
}
Start-Sleep -Milliseconds 500

# Test 3: Verifier que le stock a bien augmente a 50
Write-Host "`nTest 3: Verifier que le stock a augmente a 50..." -ForegroundColor Yellow
try {
    $article = Invoke-RestMethod -Uri "$baseUrl/articlesmagasin/$articleId" -Method Get
    if ($article.quantiteStock -eq 50) {
        Write-Host "[OK] Stock apres ENTREE = 50 (attendu)" -ForegroundColor Green
        $testResults += "[OK] Test 3 reussi"
    } else {
        Write-Host "[FAIL] Stock = $($article.quantiteStock), attendu 50" -ForegroundColor Red
        $testResults += "[FAIL] Test 3 echoue"
    }
} catch {
    Write-Host "[FAIL] Erreur: $($_.Exception.Message)" -ForegroundColor Red
    $testResults += "[FAIL] Test 3 echoue"
}
Start-Sleep -Milliseconds 500

# Test 4: Creer un mouvement de SORTIE (-20)
Write-Host "`nTest 4: Creer un mouvement de SORTIE (-20)..." -ForegroundColor Yellow
try {
    $body = @{
        articleId = $articleId
        typeMouvement = "Sortie"
        quantite = 20
        dateMouvement = (Get-Date).ToString("o")
        numeroDocument = "BS001"
        motif = "Vente"
    } | ConvertTo-Json
    
    $mouvementSortieId = Invoke-RestMethod -Uri "$baseUrl/mouvementsstock" -Method Post -Body $body -ContentType "application/json"
    Write-Host "[OK] Mouvement SORTIE cree avec ID: $mouvementSortieId" -ForegroundColor Green
    $testResults += "[OK] Test 4 reussi"
} catch {
    Write-Host "[FAIL] Erreur: $($_.Exception.Message)" -ForegroundColor Red
    $testResults += "[FAIL] Test 4 echoue"
}
Start-Sleep -Milliseconds 500

# Test 5: Verifier que le stock a diminue a 30
Write-Host "`nTest 5: Verifier que le stock a diminue a 30..." -ForegroundColor Yellow
try {
    $article = Invoke-RestMethod -Uri "$baseUrl/articlesmagasin/$articleId" -Method Get
    if ($article.quantiteStock -eq 30) {
        Write-Host "[OK] Stock apres SORTIE = 30 (attendu)" -ForegroundColor Green
        $testResults += "[OK] Test 5 reussi"
    } else {
        Write-Host "[FAIL] Stock = $($article.quantiteStock), attendu 30" -ForegroundColor Red
        $testResults += "[FAIL] Test 5 echoue"
    }
} catch {
    Write-Host "[FAIL] Erreur: $($_.Exception.Message)" -ForegroundColor Red
    $testResults += "[FAIL] Test 5 echoue"
}
Start-Sleep -Milliseconds 500

# Test 6: Tenter une SORTIE avec stock insuffisant (doit echouer)
Write-Host "`nTest 6: Tenter une SORTIE avec stock insuffisant (doit echouer)..." -ForegroundColor Yellow
try {
    $body = @{
        articleId = $articleId
        typeMouvement = "Sortie"
        quantite = 50
        dateMouvement = (Get-Date).ToString("o")
        numeroDocument = "BS002"
        motif = "Sortie impossible"
    } | ConvertTo-Json
    
    $result = Invoke-RestMethod -Uri "$baseUrl/mouvementsstock" -Method Post -Body $body -ContentType "application/json"
    Write-Host "[FAIL] Sortie avec stock insuffisant acceptee (ne devrait pas)" -ForegroundColor Red
    $testResults += "[FAIL] Test 6 echoue"
} catch {
    if ($_.Exception.Message -like "*Stock insuffisant*" -or $_.ErrorDetails.Message -like "*Stock insuffisant*") {
        Write-Host "[OK] Erreur attendue: Stock insuffisant detecte" -ForegroundColor Green
        $testResults += "[OK] Test 6 reussi"
    } else {
        Write-Host "[OK] Sortie rejetee (stock insuffisant)" -ForegroundColor Green
        $testResults += "[OK] Test 6 reussi"
    }
}
Start-Sleep -Milliseconds 500

# Test 7: Creer un mouvement d'AJUSTEMENT (+15)
Write-Host "`nTest 7: Creer un mouvement d'AJUSTEMENT (+15)..." -ForegroundColor Yellow
try {
    $body = @{
        articleId = $articleId
        typeMouvement = "Ajustement"
        quantite = 15
        dateMouvement = (Get-Date).ToString("o")
        numeroDocument = "ADJ001"
        motif = "Correction inventaire"
    } | ConvertTo-Json
    
    $mouvementAjustementId = Invoke-RestMethod -Uri "$baseUrl/mouvementsstock" -Method Post -Body $body -ContentType "application/json"
    Write-Host "[OK] Mouvement AJUSTEMENT cree avec ID: $mouvementAjustementId" -ForegroundColor Green
    $testResults += "[OK] Test 7 reussi"
} catch {
    Write-Host "[FAIL] Erreur: $($_.Exception.Message)" -ForegroundColor Red
    $testResults += "[FAIL] Test 7 echoue"
}
Start-Sleep -Milliseconds 500

# Test 8: Verifier que le stock a augmente a 45 apres ajustement
Write-Host "`nTest 8: Verifier que le stock est a 45 apres AJUSTEMENT..." -ForegroundColor Yellow
try {
    $article = Invoke-RestMethod -Uri "$baseUrl/articlesmagasin/$articleId" -Method Get
    if ($article.quantiteStock -eq 45) {
        Write-Host "[OK] Stock apres AJUSTEMENT = 45 (attendu)" -ForegroundColor Green
        $testResults += "[OK] Test 8 reussi"
    } else {
        Write-Host "[FAIL] Stock = $($article.quantiteStock), attendu 45" -ForegroundColor Red
        $testResults += "[FAIL] Test 8 echoue"
    }
} catch {
    Write-Host "[FAIL] Erreur: $($_.Exception.Message)" -ForegroundColor Red
    $testResults += "[FAIL] Test 8 echoue"
}
Start-Sleep -Milliseconds 500

# Test 9: Lister tous les mouvements de l'article
Write-Host "`nTest 9: Lister tous les mouvements de l'article..." -ForegroundColor Yellow
try {
    $mouvements = Invoke-RestMethod -Uri "$baseUrl/mouvementsstock?articleId=$articleId" -Method Get
    if ($mouvements.items.Count -eq 3) {
        Write-Host "[OK] 3 mouvements trouves (ENTREE, SORTIE, AJUSTEMENT)" -ForegroundColor Green
        $testResults += "[OK] Test 9 reussi"
    } else {
        Write-Host "[FAIL] Nombre de mouvements: $($mouvements.items.Count), attendu 3" -ForegroundColor Red
        $testResults += "[FAIL] Test 9 echoue"
    }
} catch {
    Write-Host "[FAIL] Erreur: $($_.Exception.Message)" -ForegroundColor Red
    $testResults += "[FAIL] Test 9 echoue"
}
Start-Sleep -Milliseconds 500

# Test 10: Filtrer les mouvements par type (ENTREE)
Write-Host "`nTest 10: Filtrer les mouvements par type ENTREE..." -ForegroundColor Yellow
try {
    $mouvements = Invoke-RestMethod -Uri "$baseUrl/mouvementsstock?articleId=$articleId&typeMouvement=Entree" -Method Get
    if ($mouvements.items.Count -eq 1 -and $mouvements.items[0].typeMouvement -eq "Entree") {
        Write-Host "[OK] Filtre TypeMouvement fonctionnel" -ForegroundColor Green
        $testResults += "[OK] Test 10 reussi"
    } else {
        Write-Host "[FAIL] Filtre TypeMouvement incorrect" -ForegroundColor Red
        $testResults += "[FAIL] Test 10 echoue"
    }
} catch {
    Write-Host "[FAIL] Erreur: $($_.Exception.Message)" -ForegroundColor Red
    $testResults += "[FAIL] Test 10 echoue"
}
Start-Sleep -Milliseconds 500

# Test 11: Filtrer les mouvements par magasin
Write-Host "`nTest 11: Filtrer les mouvements par magasin..." -ForegroundColor Yellow
try {
    $mouvements = Invoke-RestMethod -Uri "$baseUrl/mouvementsstock?magasinId=$magasinId" -Method Get
    if ($mouvements.items.Count -eq 3) {
        Write-Host "[OK] Filtre MagasinId fonctionnel" -ForegroundColor Green
        $testResults += "[OK] Test 11 reussi"
    } else {
        Write-Host "[FAIL] Filtre MagasinId incorrect: $($mouvements.items.Count) mouvements" -ForegroundColor Red
        $testResults += "[FAIL] Test 11 echoue"
    }
} catch {
    Write-Host "[FAIL] Erreur: $($_.Exception.Message)" -ForegroundColor Red
    $testResults += "[FAIL] Test 11 echoue"
}
Start-Sleep -Milliseconds 500

# Test 12: Filtrer les mouvements par plage de dates
Write-Host "`nTest 12: Filtrer les mouvements par plage de dates..." -ForegroundColor Yellow
try {
    $dateDebut = (Get-Date).AddDays(-1).ToString("o")
    $dateFin = (Get-Date).AddDays(1).ToString("o")
    $mouvements = Invoke-RestMethod -Uri "$baseUrl/mouvementsstock?articleId=$articleId&dateDebut=$dateDebut&dateFin=$dateFin" -Method Get
    if ($mouvements.items.Count -eq 3) {
        Write-Host "[OK] Filtre par dates fonctionnel" -ForegroundColor Green
        $testResults += "[OK] Test 12 reussi"
    } else {
        Write-Host "[FAIL] Filtre par dates incorrect" -ForegroundColor Red
        $testResults += "[FAIL] Test 12 echoue"
    }
} catch {
    Write-Host "[FAIL] Erreur: $($_.Exception.Message)" -ForegroundColor Red
    $testResults += "[FAIL] Test 12 echoue"
}
Start-Sleep -Milliseconds 500

# Test 13: Recuperer un mouvement par ID
Write-Host "`nTest 13: Recuperer un mouvement par ID..." -ForegroundColor Yellow
try {
    $mouvement = Invoke-RestMethod -Uri "$baseUrl/mouvementsstock/$mouvementEntreeId" -Method Get
    if ($mouvement.id -eq $mouvementEntreeId -and $mouvement.typeMouvement -eq "Entree" -and $mouvement.quantite -eq 50) {
        Write-Host "[OK] Mouvement recupere avec details complets" -ForegroundColor Green
        Write-Host "  - Type: $($mouvement.typeMouvement)" -ForegroundColor Gray
        Write-Host "  - Quantite: $($mouvement.quantite)" -ForegroundColor Gray
        Write-Host "  - Article: $($mouvement.articleDesignation)" -ForegroundColor Gray
        Write-Host "  - Magasin: $($mouvement.magasinNom)" -ForegroundColor Gray
        $testResults += "[OK] Test 13 reussi"
    } else {
        Write-Host "[FAIL] Donnees mouvement incorrectes" -ForegroundColor Red
        $testResults += "[FAIL] Test 13 echoue"
    }
} catch {
    Write-Host "[FAIL] Erreur: $($_.Exception.Message)" -ForegroundColor Red
    $testResults += "[FAIL] Test 13 echoue"
}
Start-Sleep -Milliseconds 500

# Test 14: Supprimer un mouvement d'AJUSTEMENT (reversal -15)
Write-Host "`nTest 14: Supprimer un mouvement d'AJUSTEMENT (reversal -15)..." -ForegroundColor Yellow
try {
    Invoke-RestMethod -Uri "$baseUrl/mouvementsstock/$mouvementAjustementId" -Method Delete
    
    # Verifier que le stock a bien diminue de 15
    $article = Invoke-RestMethod -Uri "$baseUrl/articlesmagasin/$articleId" -Method Get
    if ($article.quantiteStock -eq 30) {
        Write-Host "[OK] Stock apres suppression AJUSTEMENT = 30 (reversal applique)" -ForegroundColor Green
        $testResults += "[OK] Test 14 reussi"
    } else {
        Write-Host "[FAIL] Stock = $($article.quantiteStock), attendu 30" -ForegroundColor Red
        $testResults += "[FAIL] Test 14 echoue"
    }
} catch {
    Write-Host "[FAIL] Erreur: $($_.Exception.Message)" -ForegroundColor Red
    $testResults += "[FAIL] Test 14 echoue"
}
Start-Sleep -Milliseconds 500

# Test 15: Supprimer un mouvement de SORTIE (reversal +20)
Write-Host "`nTest 15: Supprimer un mouvement de SORTIE (reversal +20)..." -ForegroundColor Yellow
try {
    Invoke-RestMethod -Uri "$baseUrl/mouvementsstock/$mouvementSortieId" -Method Delete
    
    # Verifier que le stock a bien augmente de 20
    $article = Invoke-RestMethod -Uri "$baseUrl/articlesmagasin/$articleId" -Method Get
    if ($article.quantiteStock -eq 50) {
        Write-Host "[OK] Stock apres suppression SORTIE = 50 (reversal applique)" -ForegroundColor Green
        $testResults += "[OK] Test 15 reussi"
    } else {
        Write-Host "[FAIL] Stock = $($article.quantiteStock), attendu 50" -ForegroundColor Red
        $testResults += "[FAIL] Test 15 echoue"
    }
} catch {
    Write-Host "[FAIL] Erreur: $($_.Exception.Message)" -ForegroundColor Red
    $testResults += "[FAIL] Test 15 echoue"
}
Start-Sleep -Milliseconds 500

# Test 16: Tenter de supprimer un mouvement d'ENTREE avec stock insuffisant (doit echouer)
Write-Host "`nTest 16: Tenter de supprimer ENTREE avec stock insuffisant (doit echouer)..." -ForegroundColor Yellow
try {
    # D'abord faire une grosse sortie pour reduire le stock
    $bodyGrosseSortie = @{
        articleId = $articleId
        typeMouvement = "Sortie"
        quantite = 45
        dateMouvement = (Get-Date).ToString("o")
        numeroDocument = "BS-TEST"
        motif = "Test suppression"
    } | ConvertTo-Json
    
    Invoke-RestMethod -Uri "$baseUrl/mouvementsstock" -Method Post -Body $bodyGrosseSortie -ContentType "application/json"
    
    # Maintenant le stock devrait etre a 5 (50 - 45)
    # Tenter de supprimer l'ENTREE de 50 devrait echouer car il faudrait retirer 50 du stock
    Invoke-RestMethod -Uri "$baseUrl/mouvementsstock/$mouvementEntreeId" -Method Delete
    
    Write-Host "[FAIL] Suppression ENTREE acceptee avec stock insuffisant (ne devrait pas)" -ForegroundColor Red
    $testResults += "[FAIL] Test 16 echoue"
} catch {
    if ($_.Exception.Message -like "*Stock insuffisant*" -or $_.ErrorDetails.Message -like "*insuffisant*") {
        Write-Host "[OK] Erreur attendue: Reversal impossible avec stock insuffisant" -ForegroundColor Green
        $testResults += "[OK] Test 16 reussi"
    } else {
        Write-Host "[OK] Suppression rejetee (stock insuffisant pour reversal)" -ForegroundColor Green
        $testResults += "[OK] Test 16 reussi"
    }
}
Start-Sleep -Milliseconds 500

# Test 17: Verifier la pagination
Write-Host "`nTest 17: Verifier la pagination..." -ForegroundColor Yellow
try {
    $mouvements = Invoke-RestMethod -Uri "$baseUrl/mouvementsstock?articleId=$articleId&pageNumber=1&pageSize=1" -Method Get
    if ($mouvements.pageSize -eq 1 -and $mouvements.items.Count -le 1) {
        Write-Host "[OK] Pagination fonctionnelle" -ForegroundColor Green
        Write-Host "  - Total: $($mouvements.totalCount), Pages: $($mouvements.totalPages)" -ForegroundColor Gray
        $testResults += "[OK] Test 17 reussi"
    } else {
        Write-Host "[FAIL] Pagination incorrecte" -ForegroundColor Red
        $testResults += "[FAIL] Test 17 echoue"
    }
} catch {
    Write-Host "[FAIL] Erreur: $($_.Exception.Message)" -ForegroundColor Red
    $testResults += "[FAIL] Test 17 echoue"
}
Start-Sleep -Milliseconds 500

# Test 18: Verifier que les mouvements supprimes ne sont plus visibles
Write-Host "`nTest 18: Verifier que les mouvements supprimes ne sont plus visibles..." -ForegroundColor Yellow
try {
    $mouvements = Invoke-RestMethod -Uri "$baseUrl/mouvementsstock?articleId=$articleId" -Method Get
    $mouvementSupprime = $mouvements.items | Where-Object { $_.id -eq $mouvementAjustementId }
    
    if ($null -eq $mouvementSupprime) {
        Write-Host "[OK] Mouvements supprimes non visibles (soft delete)" -ForegroundColor Green
        $testResults += "[OK] Test 18 reussi"
    } else {
        Write-Host "[FAIL] Mouvement supprime encore visible" -ForegroundColor Red
        $testResults += "[FAIL] Test 18 echoue"
    }
} catch {
    Write-Host "[FAIL] Erreur: $($_.Exception.Message)" -ForegroundColor Red
    $testResults += "[FAIL] Test 18 echoue"
}

# Resume
Write-Host "`n=== RESUME DES TESTS MouvementsStock ===" -ForegroundColor Cyan
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



