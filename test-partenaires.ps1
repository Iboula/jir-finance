# Script de test pour le module Partenaires
$baseUrl = "http://localhost:5263/api/partenaires"
$testResults = @()

Write-Host "=== Tests Module Partenaires ===" -ForegroundColor Cyan
Write-Host ""

# Test 1: Creer un Fournisseur
Write-Host "Test 1: Creer un Fournisseur..." -ForegroundColor Yellow
try {
    $body = @{
        code = "FOUR001"
        nom = "Fournisseur Test 1"
        typePartenaire = "Fournisseur"
        adresse = "123 Rue Test"
        telephone = "0123456789"
        email = "four1@test.com"
        contactNom = "Contact 1"
        observations = "Test observations"
        isActif = $true
    } | ConvertTo-Json
    
    $fournisseur1 = Invoke-RestMethod -Uri $baseUrl -Method Post -Body $body -ContentType "application/json"
    Write-Host "[OK] Fournisseur cree: $fournisseur1" -ForegroundColor Green
    $testResults += "[OK] Test 1 reussi"
} catch {
    Write-Host "[FAIL] Erreur: $($_.Exception.Message)" -ForegroundColor Red
    $testResults += "[FAIL] Test 1 echoue"
}
Start-Sleep -Milliseconds 500

# Test 2: Creer un Sponsor
Write-Host "`nTest 2: Creer un Sponsor..." -ForegroundColor Yellow
try {
    $body = @{
        code = "SPON001"
        nom = "Sponsor Test 1"
        typePartenaire = "Sponsor"
        telephone = "0987654321"
        email = "sponsor@test.com"
        isActif = $true
    } | ConvertTo-Json
    
    $sponsor1 = Invoke-RestMethod -Uri $baseUrl -Method Post -Body $body -ContentType "application/json"
    Write-Host "[OK] Sponsor cree: $sponsor1" -ForegroundColor Green
    $testResults += "[OK] Test 2 reussi"
} catch {
    Write-Host "[FAIL] Erreur: $($_.Exception.Message)" -ForegroundColor Red
    $testResults += "[FAIL] Test 2 echoue"
}
Start-Sleep -Milliseconds 500

# Test 3: Creer un Client
Write-Host "`nTest 3: Creer un Client..." -ForegroundColor Yellow
try {
    $body = @{
        code = "CLI001"
        nom = "Client Test 1"
        typePartenaire = "Client"
        adresse = "456 Avenue Test"
        telephone = "0111222333"
        isActif = $true
    } | ConvertTo-Json
    
    $client1 = Invoke-RestMethod -Uri $baseUrl -Method Post -Body $body -ContentType "application/json"
    Write-Host "[OK] Client cree: $client1" -ForegroundColor Green
    $testResults += "[OK] Test 3 reussi"
} catch {
    Write-Host "[FAIL] Erreur: $($_.Exception.Message)" -ForegroundColor Red
    $testResults += "[FAIL] Test 3 echoue"
}
Start-Sleep -Milliseconds 500

# Test 4: Creer un partenaire Autre
Write-Host "`nTest 4: Creer un partenaire type Autre..." -ForegroundColor Yellow
try {
    $body = @{
        code = "AUT001"
        nom = "Autre Test 1"
        typePartenaire = "Autre"
        isActif = $true
    } | ConvertTo-Json
    
    $autre1 = Invoke-RestMethod -Uri $baseUrl -Method Post -Body $body -ContentType "application/json"
    Write-Host "[OK] Partenaire Autre cree: $autre1" -ForegroundColor Green
    $testResults += "[OK] Test 4 reussi"
} catch {
    Write-Host "[FAIL] Erreur: $($_.Exception.Message)" -ForegroundColor Red
    $testResults += "[FAIL] Test 4 echoue"
}
Start-Sleep -Milliseconds 500

# Test 5: Tester unicite du code (doit echouer)
Write-Host "`nTest 5: Tester unicite du code (doit echouer)..." -ForegroundColor Yellow
try {
    $body = @{
        code = "FOUR001"
        nom = "Fournisseur Doublon"
        typePartenaire = "Fournisseur"
        isActif = $true
    } | ConvertTo-Json
    
    Invoke-RestMethod -Uri $baseUrl -Method Post -Body $body -ContentType "application/json"
    Write-Host "[FAIL] Le code en doublon n'a pas ete rejete" -ForegroundColor Red
    $testResults += "[FAIL] Test 5 echoue"
} catch {
    Write-Host "[OK] Code en doublon rejete comme attendu" -ForegroundColor Green
    $testResults += "[OK] Test 5 reussi"
}
Start-Sleep -Milliseconds 500

# Test 6: Lister tous les partenaires
Write-Host "`nTest 6: Lister tous les partenaires..." -ForegroundColor Yellow
try {
    $partenaires = Invoke-RestMethod -Uri $baseUrl -Method Get
    Write-Host "[OK] Liste recuperee: $($partenaires.totalCount) partenaires" -ForegroundColor Green
    $testResults += "[OK] Test 6 reussi"
} catch {
    Write-Host "[FAIL] Erreur: $($_.Exception.Message)" -ForegroundColor Red
    $testResults += "[FAIL] Test 6 echoue"
}
Start-Sleep -Milliseconds 500

# Test 7: Filtrer par type Fournisseur
Write-Host "`nTest 7: Filtrer par type Fournisseur..." -ForegroundColor Yellow
try {
    $fournisseurs = Invoke-RestMethod -Uri "$baseUrl`?typePartenaire=Fournisseur" -Method Get
    Write-Host "[OK] Fournisseurs trouves: $($fournisseurs.totalCount)" -ForegroundColor Green
    $testResults += "[OK] Test 7 reussi"
} catch {
    Write-Host "[FAIL] Erreur: $($_.Exception.Message)" -ForegroundColor Red
    $testResults += "[FAIL] Test 7 echoue"
}
Start-Sleep -Milliseconds 500

# Test 8: Filtrer par IsActif = true
Write-Host "`nTest 8: Filtrer par IsActif = true..." -ForegroundColor Yellow
try {
    $actifs = Invoke-RestMethod -Uri "$baseUrl`?isActif=true" -Method Get
    Write-Host "[OK] Partenaires actifs: $($actifs.totalCount)" -ForegroundColor Green
    $testResults += "[OK] Test 8 reussi"
} catch {
    Write-Host "[FAIL] Erreur: $($_.Exception.Message)" -ForegroundColor Red
    $testResults += "[FAIL] Test 8 echoue"
}
Start-Sleep -Milliseconds 500

# Test 9: Recherche par code
Write-Host "`nTest 9: Recherche par code 'FOUR'..." -ForegroundColor Yellow
try {
    $search = Invoke-RestMethod -Uri "$baseUrl`?search=FOUR" -Method Get
    Write-Host "[OK] Resultats de recherche: $($search.totalCount)" -ForegroundColor Green
    $testResults += "[OK] Test 9 reussi"
} catch {
    Write-Host "[FAIL] Erreur: $($_.Exception.Message)" -ForegroundColor Red
    $testResults += "[FAIL] Test 9 echoue"
}
Start-Sleep -Milliseconds 500

# Test 10: Obtenir un partenaire par ID
Write-Host "`nTest 10: Obtenir un partenaire par ID..." -ForegroundColor Yellow
try {
    $partenaire = Invoke-RestMethod -Uri "$baseUrl/$fournisseur1" -Method Get
    Write-Host "[OK] Partenaire recupere: $($partenaire.nom)" -ForegroundColor Green
    Write-Host "  - Code: $($partenaire.code)" -ForegroundColor Gray
    Write-Host "  - Type: $($partenaire.typePartenaire)" -ForegroundColor Gray
    Write-Host "  - Email: $($partenaire.email)" -ForegroundColor Gray
    $testResults += "[OK] Test 10 reussi"
} catch {
    Write-Host "[FAIL] Erreur: $($_.Exception.Message)" -ForegroundColor Red
    $testResults += "[FAIL] Test 10 echoue"
}
Start-Sleep -Milliseconds 500

# Test 11: Mettre a jour un partenaire
Write-Host "`nTest 11: Mettre a jour un partenaire..." -ForegroundColor Yellow
try {
    $body = @{
        id = $fournisseur1
        code = "FOUR001"
        nom = "Fournisseur Test 1 - Modifie"
        typePartenaire = "Fournisseur"
        adresse = "789 Boulevard Modifie"
        telephone = "0999888777"
        email = "four1_updated@test.com"
        contactNom = "Contact Modifie"
        observations = "Observations modifiees"
        isActif = $true
    } | ConvertTo-Json
    
    Invoke-RestMethod -Uri "$baseUrl/$fournisseur1" -Method Put -Body $body -ContentType "application/json"
    Write-Host "[OK] Partenaire mis a jour" -ForegroundColor Green
    $testResults += "[OK] Test 11 reussi"
} catch {
    Write-Host "[FAIL] Erreur: $($_.Exception.Message)" -ForegroundColor Red
    $testResults += "[FAIL] Test 11 echoue"
}
Start-Sleep -Milliseconds 500

# Test 12: Verifier la mise a jour
Write-Host "`nTest 12: Verifier la mise a jour..." -ForegroundColor Yellow
try {
    $updated = Invoke-RestMethod -Uri "$baseUrl/$fournisseur1" -Method Get
    if ($updated.nom -eq "Fournisseur Test 1 - Modifie" -and $updated.email -eq "four1_updated@test.com") {
        Write-Host "[OK] Mise a jour verifiee: $($updated.nom)" -ForegroundColor Green
        $testResults += "[OK] Test 12 reussi"
    } else {
        Write-Host "[FAIL] Les modifications ne sont pas correctes" -ForegroundColor Red
        $testResults += "[FAIL] Test 12 echoue"
    }
} catch {
    Write-Host "[FAIL] Erreur: $($_.Exception.Message)" -ForegroundColor Red
    $testResults += "[FAIL] Test 12 echoue"
}
Start-Sleep -Milliseconds 500

# Test 13: Desactiver un partenaire
Write-Host "`nTest 13: Desactiver un partenaire..." -ForegroundColor Yellow
try {
    $body = @{
        id = $client1
        code = "CLI001"
        nom = "Client Test 1"
        typePartenaire = "Client"
        adresse = "456 Avenue Test"
        telephone = "0111222333"
        isActif = $false
    } | ConvertTo-Json
    
    Invoke-RestMethod -Uri "$baseUrl/$client1" -Method Put -Body $body -ContentType "application/json"
    Write-Host "[OK] Partenaire desactive" -ForegroundColor Green
    $testResults += "[OK] Test 13 reussi"
} catch {
    Write-Host "[FAIL] Erreur: $($_.Exception.Message)" -ForegroundColor Red
    $testResults += "[FAIL] Test 13 echoue"
}
Start-Sleep -Milliseconds 500

# Test 14: Filtrer les partenaires inactifs
Write-Host "`nTest 14: Filtrer les partenaires inactifs..." -ForegroundColor Yellow
try {
    $inactifs = Invoke-RestMethod -Uri "$baseUrl`?isActif=false" -Method Get
    Write-Host "[OK] Partenaires inactifs: $($inactifs.totalCount)" -ForegroundColor Green
    $testResults += "[OK] Test 14 reussi"
} catch {
    Write-Host "[FAIL] Erreur: $($_.Exception.Message)" -ForegroundColor Red
    $testResults += "[FAIL] Test 14 echoue"
}
Start-Sleep -Milliseconds 500

# Test 15: Supprimer un partenaire (soft delete)
Write-Host "`nTest 15: Supprimer un partenaire (soft delete)..." -ForegroundColor Yellow
try {
    Invoke-RestMethod -Uri "$baseUrl/$autre1" -Method Delete
    Write-Host "[OK] Partenaire supprime" -ForegroundColor Green
    $testResults += "[OK] Test 15 reussi"
} catch {
    Write-Host "[FAIL] Erreur: $($_.Exception.Message)" -ForegroundColor Red
    $testResults += "[FAIL] Test 15 echoue"
}
Start-Sleep -Milliseconds 500

# Test 16: Verifier que le partenaire supprime n'apparait plus
Write-Host "`nTest 16: Verifier que le partenaire supprime n'apparait plus..." -ForegroundColor Yellow
try {
    Invoke-RestMethod -Uri "$baseUrl/$autre1" -Method Get
    Write-Host "[FAIL] Le partenaire supprime est toujours accessible" -ForegroundColor Red
    $testResults += "[FAIL] Test 16 echoue"
} catch {
    Write-Host "[OK] Le partenaire supprime n'est plus accessible (soft delete OK)" -ForegroundColor Green
    $testResults += "[OK] Test 16 reussi"
}

# Resume
Write-Host "`n=== RESUME DES TESTS ===" -ForegroundColor Cyan
$passed = ($testResults | Where-Object { $_ -like "[OK]*" }).Count
$total = $testResults.Count
Write-Host "Tests reussis: $passed/$total" -ForegroundColor $(if ($passed -eq $total) { "Green" } else { "Yellow" })
Write-Host ""
$testResults | ForEach-Object { Write-Host $_ }
