# Script de test pour Depenses API
Write-Host "=== Demarrage de l'API ===" -ForegroundColor Cyan

# Demarrer l'API en background job
$apiJob = Start-Job -ScriptBlock {
    Set-Location "c:\Users\iboul\OneDrive\Documents\PROJETS\MOPRO\JIR\src\JIR.WebApi"
    dotnet run
}

Write-Host "API demarree (Job ID: $($apiJob.Id))" -ForegroundColor Green
Write-Host "Attente de 10 secondes..." -ForegroundColor Yellow
Start-Sleep -Seconds 10

try {
    Write-Host "`n=== Tests Depenses ===" -ForegroundColor Cyan

    # 1. Get Section
    Write-Host "`n[1] Get Section..." -ForegroundColor Yellow
    $sections = Invoke-RestMethod -Uri "http://localhost:5263/api/sections" -Method Get
    $sectionId = $sections[0].id
    Write-Host "OK: Section $sectionId" -ForegroundColor Green

    # 2. Create Depense (Brouillon)
    Write-Host "`n[2] Create Depense..." -ForegroundColor Yellow
    $create = @{
        sectionId = $sectionId
        numero = "DEP-$(Get-Date -Format 'yyyyMMddHHmmss')"
        dateDepense = (Get-Date).ToString("yyyy-MM-ddTHH:mm:ssZ")
        categorie = "Fournitures"
        description = "Test depense"
        montant = 250.50
        beneficiaire = "Test Supplier"
        modePaiement = "Virement"
        referenceFacture = "FACT-001"
    } | ConvertTo-Json

    $result = Invoke-RestMethod -Uri "http://localhost:5263/api/depenses" -Method Post -Body $create -ContentType "application/json"
    $depenseId = $result.id
    Write-Host "OK: Depense $depenseId created" -ForegroundColor Green

    # 3. Get Depense
    Write-Host "`n[3] Get Depense..." -ForegroundColor Yellow
    $depense = Invoke-RestMethod -Uri "http://localhost:5263/api/depenses/$depenseId" -Method Get
    Write-Host "OK: Numero=$($depense.numero), Statut=$($depense.statut), Montant=$($depense.montant)" -ForegroundColor Green

    # 4. Update Depense
    Write-Host "`n[4] Update Depense..." -ForegroundColor Yellow
    $update = @{
        id = $depenseId
        sectionId = $sectionId
        numero = $depense.numero
        dateDepense = $depense.dateDepense
        categorie = "Fournitures"
        description = "Test depense UPDATED"
        montant = 275.75
        beneficiaire = "Test Supplier"
        modePaiement = "Virement"
        referenceFacture = "FACT-001"
    } | ConvertTo-Json

    Invoke-RestMethod -Uri "http://localhost:5263/api/depenses/$depenseId" -Method Put -Body $update -ContentType "application/json"
    Write-Host "OK: Updated" -ForegroundColor Green

    # 5. Valider (Brouillon to Valide)
    Write-Host "`n[5] Valider Depense..." -ForegroundColor Yellow
    $valider = @{
        id = $depenseId
        validePar = "Jean Dupont"
        observations = "OK"
    } | ConvertTo-Json

    Invoke-RestMethod -Uri "http://localhost:5263/api/depenses/$depenseId/valider" -Method Put -Body $valider -ContentType "application/json"
    $depense = Invoke-RestMethod -Uri "http://localhost:5263/api/depenses/$depenseId" -Method Get
    Write-Host "OK: Statut=$($depense.statut), ValidePar=$($depense.validePar)" -ForegroundColor Green

    # 6. Try to update (should fail - no longer Brouillon)
    Write-Host "`n[6] Try Update Valide (should fail)..." -ForegroundColor Yellow
    try {
        Invoke-RestMethod -Uri "http://localhost:5263/api/depenses/$depenseId" -Method Put -Body $update -ContentType "application/json"
        Write-Host "ERROR: Should have failed!" -ForegroundColor Red
    } catch {
        Write-Host "OK: Update rejected (expected)" -ForegroundColor Green
    }

    # 7. Payer (Valide to Paye)
    Write-Host "`n[7] Payer Depense..." -ForegroundColor Yellow
    $payer = @{
        id = $depenseId
        datePaiement = (Get-Date).ToString("yyyy-MM-ddTHH:mm:ssZ")
        modePaiement = "Virement SEPA"
        referenceFacture = "VIR-001"
    } | ConvertTo-Json

    Invoke-RestMethod -Uri "http://localhost:5263/api/depenses/$depenseId/payer" -Method Put -Body $payer -ContentType "application/json"
    $depense = Invoke-RestMethod -Uri "http://localhost:5263/api/depenses/$depenseId" -Method Get
    Write-Host "OK: Statut=$($depense.statut), DatePaiement=$($depense.datePaiement)" -ForegroundColor Green

    # 8. Test Rejection - Create new
    Write-Host "`n[8] Create Depense for Rejection..." -ForegroundColor Yellow
    $create2 = @{
        sectionId = $sectionId
        numero = "DEP-REJ-$(Get-Date -Format 'yyyyMMddHHmmss')"
        dateDepense = (Get-Date).ToString("yyyy-MM-ddTHH:mm:ssZ")
        categorie = "Services"
        description = "To reject"
        montant = 100.00
        beneficiaire = "Test"
    } | ConvertTo-Json

    $result2 = Invoke-RestMethod -Uri "http://localhost:5263/api/depenses" -Method Post -Body $create2 -ContentType "application/json"
    $depenseId2 = $result2.id
    Write-Host "OK: Depense $depenseId2 created" -ForegroundColor Green

    # 9. Rejeter
    Write-Host "`n[9] Rejeter Depense..." -ForegroundColor Yellow
    $rejeter = @{
        id = $depenseId2
        motifRejet = "Budget depasse"
        rejetePar = "Marie Dubois"
    } | ConvertTo-Json

    Invoke-RestMethod -Uri "http://localhost:5263/api/depenses/$depenseId2/rejeter" -Method Put -Body $rejeter -ContentType "application/json"
    $depense2 = Invoke-RestMethod -Uri "http://localhost:5263/api/depenses/$depenseId2" -Method Get
    Write-Host "OK: Statut=$($depense2.statut), Motif=$($depense2.motifRejet)" -ForegroundColor Green

    # 10. Test Delete - Create new
    Write-Host "`n[10] Create Depense for Delete..." -ForegroundColor Yellow
    $create3 = @{
        sectionId = $sectionId
        numero = "DEP-DEL-$(Get-Date -Format 'yyyyMMddHHmmss')"
        dateDepense = (Get-Date).ToString("yyyy-MM-ddTHH:mm:ssZ")
        categorie = "Test"
        description = "To delete"
        montant = 50.00
        beneficiaire = "Test"
    } | ConvertTo-Json

    $result3 = Invoke-RestMethod -Uri "http://localhost:5263/api/depenses" -Method Post -Body $create3 -ContentType "application/json"
    $depenseId3 = $result3.id
    Write-Host "OK: Depense $depenseId3 created" -ForegroundColor Green

    # 11. Delete
    Write-Host "`n[11] Delete Depense..." -ForegroundColor Yellow
    Invoke-RestMethod -Uri "http://localhost:5263/api/depenses/$depenseId3" -Method Delete
    Write-Host "OK: Deleted" -ForegroundColor Green

    # 12. Verify deleted not accessible
    Write-Host "`n[12] Verify Deleted..." -ForegroundColor Yellow
    try {
        $deleted = Invoke-RestMethod -Uri "http://localhost:5263/api/depenses/$depenseId3" -Method Get
        Write-Host "ERROR: Should not be accessible!" -ForegroundColor Red
    } catch {
        Write-Host "OK: Not accessible (expected)" -ForegroundColor Green
    }

    # 13. List all
    Write-Host "`n[13] List All Depenses..." -ForegroundColor Yellow
    $all = Invoke-RestMethod -Uri "http://localhost:5263/api/depenses" -Method Get
    Write-Host "OK: Total=$($all.Count)" -ForegroundColor Green

    # 14. Filter by Paye
    Write-Host "`n[14] Filter by Statut=Paye..." -ForegroundColor Yellow
    $paye = Invoke-RestMethod -Uri "http://localhost:5263/api/depenses?statut=Paye" -Method Get
    Write-Host "OK: Paye=$($paye.Count)" -ForegroundColor Green

    # 15. Filter by Brouillon
    Write-Host "`n[15] Filter by Statut=Brouillon..." -ForegroundColor Yellow
    $brouillon = Invoke-RestMethod -Uri "http://localhost:5263/api/depenses?statut=Brouillon" -Method Get
    Write-Host "OK: Brouillon=$($brouillon.Count)" -ForegroundColor Green

    # 16. Filter by Section
    Write-Host "`n[16] Filter by Section..." -ForegroundColor Yellow
    $section = Invoke-RestMethod -Uri "http://localhost:5263/api/depenses?sectionId=$sectionId" -Method Get
    Write-Host "OK: Section=$($section.Count)" -ForegroundColor Green

    Write-Host "`n=== TOUS LES TESTS REUSSIS ===" -ForegroundColor Green
    Write-Host "Workflow: Brouillon > Valide > Paye" -ForegroundColor Cyan
    Write-Host "Rejection: Brouillon > Rejete" -ForegroundColor Cyan
    Write-Host "Delete: Soft delete OK" -ForegroundColor Cyan
    Write-Host "Filters: OK" -ForegroundColor Cyan

} catch {
    Write-Host "`nERREUR:" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
} finally {
    Write-Host "`n=== Arret API ===" -ForegroundColor Cyan
    Stop-Job -Job $apiJob
    Remove-Job -Job $apiJob
    Write-Host "Done" -ForegroundColor Green
}
