# Script de test pour le module Magasins
$baseUrl = "http://localhost:5263/api/magasins"
$testResults = @()

Write-Host "=== Tests Module Magasins ===" -ForegroundColor Cyan
Write-Host ""

# Test 1: Creer un magasin
Write-Host "Test 1: Creer un magasin..." -ForegroundColor Yellow
try {
    $body = @{
        code = "MAG001"
        nom = "Magasin Principal"
        localisation = "Batiment A, Etage 1"
    } | ConvertTo-Json
    
    $magasin1 = Invoke-RestMethod -Uri $baseUrl -Method Post -Body $body -ContentType "application/json"
    Write-Host "[OK] Magasin cree: $magasin1" -ForegroundColor Green
    $testResults += "[OK] Test 1 reussi"
} catch {
    Write-Host "[FAIL] Erreur: $($_.Exception.Message)" -ForegroundColor Red
    $testResults += "[FAIL] Test 1 echoue"
}
Start-Sleep -Milliseconds 500

# Test 2: Creer un deuxieme magasin
Write-Host "`nTest 2: Creer un deuxieme magasin..." -ForegroundColor Yellow
try {
    $body = @{
        code = "MAG002"
        nom = "Magasin Secondaire"
        localisation = "Batiment B"
    } | ConvertTo-Json
    
    $magasin2 = Invoke-RestMethod -Uri $baseUrl -Method Post -Body $body -ContentType "application/json"
    Write-Host "[OK] Magasin cree: $magasin2" -ForegroundColor Green
    $testResults += "[OK] Test 2 reussi"
} catch {
    Write-Host "[FAIL] Erreur: $($_.Exception.Message)" -ForegroundColor Red
    $testResults += "[FAIL] Test 2 echoue"
}
Start-Sleep -Milliseconds 500

# Test 3: Creer un troisieme magasin
Write-Host "`nTest 3: Creer un troisieme magasin..." -ForegroundColor Yellow
try {
    $body = @{
        code = "MAG003"
        nom = "Depot"
        localisation = "Entrepot Zone C"
    } | ConvertTo-Json
    
    $magasin3 = Invoke-RestMethod -Uri $baseUrl -Method Post -Body $body -ContentType "application/json"
    Write-Host "[OK] Magasin cree: $magasin3" -ForegroundColor Green
    $testResults += "[OK] Test 3 reussi"
} catch {
    Write-Host "[FAIL] Erreur: $($_.Exception.Message)" -ForegroundColor Red
    $testResults += "[FAIL] Test 3 echoue"
}
Start-Sleep -Milliseconds 500

# Test 4: Tester unicite du code (doit echouer)
Write-Host "`nTest 4: Tester unicite du code (doit echouer)..." -ForegroundColor Yellow
try {
    $body = @{
        code = "MAG001"
        nom = "Magasin Doublon"
    } | ConvertTo-Json
    
    Invoke-RestMethod -Uri $baseUrl -Method Post -Body $body -ContentType "application/json"
    Write-Host "[FAIL] Le code en doublon n'a pas ete rejete" -ForegroundColor Red
    $testResults += "[FAIL] Test 4 echoue"
} catch {
    Write-Host "[OK] Code en doublon rejete comme attendu" -ForegroundColor Green
    $testResults += "[OK] Test 4 reussi"
}
Start-Sleep -Milliseconds 500

# Test 5: Lister tous les magasins
Write-Host "`nTest 5: Lister tous les magasins..." -ForegroundColor Yellow
try {
    $magasins = Invoke-RestMethod -Uri $baseUrl -Method Get
    Write-Host "[OK] Liste recuperee: $($magasins.totalCount) magasins" -ForegroundColor Green
    $testResults += "[OK] Test 5 reussi"
} catch {
    Write-Host "[FAIL] Erreur: $($_.Exception.Message)" -ForegroundColor Red
    $testResults += "[FAIL] Test 5 echoue"
}
Start-Sleep -Milliseconds 500

# Test 6: Recherche par code
Write-Host "`nTest 6: Recherche par code 'MAG'..." -ForegroundColor Yellow
try {
    $search = Invoke-RestMethod -Uri "$baseUrl`?search=MAG" -Method Get
    Write-Host "[OK] Resultats de recherche: $($search.totalCount)" -ForegroundColor Green
    $testResults += "[OK] Test 6 reussi"
} catch {
    Write-Host "[FAIL] Erreur: $($_.Exception.Message)" -ForegroundColor Red
    $testResults += "[FAIL] Test 6 echoue"
}
Start-Sleep -Milliseconds 500

# Test 7: Recherche par nom
Write-Host "`nTest 7: Recherche par nom 'Principal'..." -ForegroundColor Yellow
try {
    $search = Invoke-RestMethod -Uri "$baseUrl`?search=Principal" -Method Get
    Write-Host "[OK] Resultats de recherche: $($search.totalCount)" -ForegroundColor Green
    $testResults += "[OK] Test 7 reussi"
} catch {
    Write-Host "[FAIL] Erreur: $($_.Exception.Message)" -ForegroundColor Red
    $testResults += "[FAIL] Test 7 echoue"
}
Start-Sleep -Milliseconds 500

# Test 8: Obtenir un magasin par ID
Write-Host "`nTest 8: Obtenir un magasin par ID..." -ForegroundColor Yellow
try {
    $magasin = Invoke-RestMethod -Uri "$baseUrl/$magasin1" -Method Get
    Write-Host "[OK] Magasin recupere: $($magasin.nom)" -ForegroundColor Green
    Write-Host "  - Code: $($magasin.code)" -ForegroundColor Gray
    Write-Host "  - Localisation: $($magasin.localisation)" -ForegroundColor Gray
    $testResults += "[OK] Test 8 reussi"
} catch {
    Write-Host "[FAIL] Erreur: $($_.Exception.Message)" -ForegroundColor Red
    $testResults += "[FAIL] Test 8 echoue"
}
Start-Sleep -Milliseconds 500

# Test 9: Mettre a jour un magasin
Write-Host "`nTest 9: Mettre a jour un magasin..." -ForegroundColor Yellow
try {
    $body = @{
        id = $magasin1
        code = "MAG001"
        nom = "Magasin Principal - Modifie"
        localisation = "Batiment A, Etage 2 (nouveau)"
    } | ConvertTo-Json
    
    Invoke-RestMethod -Uri "$baseUrl/$magasin1" -Method Put -Body $body -ContentType "application/json"
    Write-Host "[OK] Magasin mis a jour" -ForegroundColor Green
    $testResults += "[OK] Test 9 reussi"
} catch {
    Write-Host "[FAIL] Erreur: $($_.Exception.Message)" -ForegroundColor Red
    $testResults += "[FAIL] Test 9 echoue"
}
Start-Sleep -Milliseconds 500

# Test 10: Verifier la mise a jour
Write-Host "`nTest 10: Verifier la mise a jour..." -ForegroundColor Yellow
try {
    $updated = Invoke-RestMethod -Uri "$baseUrl/$magasin1" -Method Get
    if ($updated.nom -eq "Magasin Principal - Modifie" -and $updated.localisation -like "*nouveau*") {
        Write-Host "[OK] Mise a jour verifiee: $($updated.nom)" -ForegroundColor Green
        $testResults += "[OK] Test 10 reussi"
    } else {
        Write-Host "[FAIL] Les modifications ne sont pas correctes" -ForegroundColor Red
        $testResults += "[FAIL] Test 10 echoue"
    }
} catch {
    Write-Host "[FAIL] Erreur: $($_.Exception.Message)" -ForegroundColor Red
    $testResults += "[FAIL] Test 10 echoue"
}
Start-Sleep -Milliseconds 500

# Test 11: Tester pagination
Write-Host "`nTest 11: Tester pagination (pageSize=2)..." -ForegroundColor Yellow
try {
    $paginated = Invoke-RestMethod -Uri "$baseUrl`?pageNumber=1&pageSize=2" -Method Get
    Write-Host "[OK] Page 1: $($paginated.magasins.Count) magasins, Total: $($paginated.totalCount)" -ForegroundColor Green
    $testResults += "[OK] Test 11 reussi"
} catch {
    Write-Host "[FAIL] Erreur: $($_.Exception.Message)" -ForegroundColor Red
    $testResults += "[FAIL] Test 11 echoue"
}
Start-Sleep -Milliseconds 500

# Test 12: Supprimer un magasin (soft delete)
Write-Host "`nTest 12: Supprimer un magasin (soft delete)..." -ForegroundColor Yellow
try {
    Invoke-RestMethod -Uri "$baseUrl/$magasin3" -Method Delete
    Write-Host "[OK] Magasin supprime" -ForegroundColor Green
    $testResults += "[OK] Test 12 reussi"
} catch {
    Write-Host "[FAIL] Erreur: $($_.Exception.Message)" -ForegroundColor Red
    $testResults += "[FAIL] Test 12 echoue"
}
Start-Sleep -Milliseconds 500

# Test 13: Verifier que le magasin supprime n'apparait plus
Write-Host "`nTest 13: Verifier que le magasin supprime n'apparait plus..." -ForegroundColor Yellow
try {
    Invoke-RestMethod -Uri "$baseUrl/$magasin3" -Method Get
    Write-Host "[FAIL] Le magasin supprime est toujours accessible" -ForegroundColor Red
    $testResults += "[FAIL] Test 13 echoue"
} catch {
    Write-Host "[OK] Le magasin supprime n'est plus accessible (soft delete OK)" -ForegroundColor Green
    $testResults += "[OK] Test 13 reussi"
}
Start-Sleep -Milliseconds 500

# Test 14: Verifier le nombre de magasins apres suppression
Write-Host "`nTest 14: Verifier le nombre de magasins apres suppression..." -ForegroundColor Yellow
try {
    $remaining = Invoke-RestMethod -Uri $baseUrl -Method Get
    if ($remaining.totalCount -eq 2) {
        Write-Host "[OK] Nombre correct: $($remaining.totalCount) magasins actifs" -ForegroundColor Green
        $testResults += "[OK] Test 14 reussi"
    } else {
        Write-Host "[FAIL] Nombre incorrect: $($remaining.totalCount) (attendu: 2)" -ForegroundColor Red
        $testResults += "[FAIL] Test 14 echoue"
    }
} catch {
    Write-Host "[FAIL] Erreur: $($_.Exception.Message)" -ForegroundColor Red
    $testResults += "[FAIL] Test 14 echoue"
}

# Resume
Write-Host "`n=== RESUME DES TESTS ===" -ForegroundColor Cyan
$passed = ($testResults | Where-Object { $_ -like "[OK]*" }).Count
$total = $testResults.Count
Write-Host "Tests reussis: $passed/$total" -ForegroundColor $(if ($passed -eq $total) { "Green" } else { "Yellow" })
Write-Host ""
$testResults | ForEach-Object { Write-Host $_ }
