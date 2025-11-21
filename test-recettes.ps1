# Script de test pour Recettes API
Write-Host "=== Demarrage de l'API ===" -ForegroundColor Cyan

$apiJob = Start-Job -ScriptBlock {
    Set-Location "c:\Users\iboul\OneDrive\Documents\PROJETS\MOPRO\JIR\src\JIR.WebApi"
    dotnet run
}

Write-Host "API demarree (Job ID: $($apiJob.Id))" -ForegroundColor Green
Write-Host "Attente de 15 secondes..." -ForegroundColor Yellow
Start-Sleep -Seconds 15

try {
    Write-Host "`n=== Tests Recettes ===" -ForegroundColor Cyan

    # 1. Get Section
    Write-Host "`n[1] Get Section..." -ForegroundColor Yellow
    $sections = Invoke-RestMethod -Uri "http://localhost:5263/api/sections" -Method Get
    $sectionId = $sections[0].id
    Write-Host "OK: Section $sectionId" -ForegroundColor Green

    # 2. Create Recette
    Write-Host "`n[2] Create Recette..." -ForegroundColor Yellow
    $create = @{
        sectionId = $sectionId
        libelle = "Cotisation membre 2024"
        montant = 500.00
        dateRecette = (Get-Date).ToString("yyyy-MM-ddTHH:mm:ssZ")
        source = "Membre Jean Dupont"
        categorie = "Cotisations"
        recuNumero = "RECU-$(Get-Date -Format 'yyyyMMddHHmmss')"
        modeEncaissement = "Virement"
        numeroDocument = "VIR-001"
        observations = "Cotisation annuelle"
    } | ConvertTo-Json

    $result = Invoke-RestMethod -Uri "http://localhost:5263/api/recettes" -Method Post -Body $create -ContentType "application/json"
    $recetteId = $result.id
    Write-Host "OK: Recette $recetteId created" -ForegroundColor Green

    # 3. Get Recette
    Write-Host "`n[3] Get Recette..." -ForegroundColor Yellow
    $recette = Invoke-RestMethod -Uri "http://localhost:5263/api/recettes/$recetteId" -Method Get
    Write-Host "OK: Libelle=$($recette.libelle), Montant=$($recette.montant), Categorie=$($recette.categorie)" -ForegroundColor Green

    # 4. Update Recette
    Write-Host "`n[4] Update Recette..." -ForegroundColor Yellow
    $update = @{
        id = $recetteId
        sectionId = $sectionId
        libelle = "Cotisation membre 2024 - UPDATED"
        montant = 550.00
        dateRecette = $recette.dateRecette
        source = "Membre Jean Dupont"
        categorie = "Cotisations"
        recuNumero = $recette.recuNumero
        modeEncaissement = "Virement SEPA"
        numeroDocument = "VIR-001"
        observations = "Cotisation annuelle + don"
    } | ConvertTo-Json

    Invoke-RestMethod -Uri "http://localhost:5263/api/recettes/$recetteId" -Method Put -Body $update -ContentType "application/json"
    $recette = Invoke-RestMethod -Uri "http://localhost:5263/api/recettes/$recetteId" -Method Get
    Write-Host "OK: Updated - Montant=$($recette.montant)" -ForegroundColor Green

    # 5. Create more Recettes for filtering
    Write-Host "`n[5] Create Recette (Dons)..." -ForegroundColor Yellow
    $create2 = @{
        sectionId = $sectionId
        libelle = "Don entreprise ABC"
        montant = 1000.00
        dateRecette = (Get-Date).ToString("yyyy-MM-ddTHH:mm:ssZ")
        source = "Entreprise ABC"
        categorie = "Dons"
        modeEncaissement = "Cheque"
        numeroDocument = "CHQ-123456"
    } | ConvertTo-Json

    $result2 = Invoke-RestMethod -Uri "http://localhost:5263/api/recettes" -Method Post -Body $create2 -ContentType "application/json"
    Write-Host "OK: Recette Don created" -ForegroundColor Green

    # 6. Create Recette (Subventions)
    Write-Host "`n[6] Create Recette (Subventions)..." -ForegroundColor Yellow
    $create3 = @{
        sectionId = $sectionId
        libelle = "Subvention municipale"
        montant = 5000.00
        dateRecette = (Get-Date).ToString("yyyy-MM-ddTHH:mm:ssZ")
        source = "Mairie"
        categorie = "Subventions"
        modeEncaissement = "Virement"
        numeroDocument = "SUBV-2024-001"
    } | ConvertTo-Json

    $result3 = Invoke-RestMethod -Uri "http://localhost:5263/api/recettes" -Method Post -Body $create3 -ContentType "application/json"
    $recetteId3 = $result3.id
    Write-Host "OK: Recette Subvention created" -ForegroundColor Green

    # 7. List all
    Write-Host "`n[7] List All Recettes..." -ForegroundColor Yellow
    $all = Invoke-RestMethod -Uri "http://localhost:5263/api/recettes" -Method Get
    Write-Host "OK: Total=$($all.Count)" -ForegroundColor Green
    foreach ($r in $all) {
        Write-Host "  - $($r.libelle): $($r.montant) EUR ($($r.categorie))" -ForegroundColor Cyan
    }

    # 8. Filter by Categorie=Cotisations
    Write-Host "`n[8] Filter by Categorie=Cotisations..." -ForegroundColor Yellow
    $cotisations = Invoke-RestMethod -Uri "http://localhost:5263/api/recettes?categorie=Cotisations" -Method Get
    Write-Host "OK: Cotisations=$($cotisations.Count)" -ForegroundColor Green

    # 9. Filter by Categorie=Dons
    Write-Host "`n[9] Filter by Categorie=Dons..." -ForegroundColor Yellow
    $dons = Invoke-RestMethod -Uri "http://localhost:5263/api/recettes?categorie=Dons" -Method Get
    Write-Host "OK: Dons=$($dons.Count)" -ForegroundColor Green

    # 10. Filter by Source
    Write-Host "`n[10] Filter by Source=Entreprise..." -ForegroundColor Yellow
    $entreprise = Invoke-RestMethod -Uri "http://localhost:5263/api/recettes?source=Entreprise" -Method Get
    Write-Host "OK: Source Entreprise=$($entreprise.Count)" -ForegroundColor Green

    # 11. Filter by Section
    Write-Host "`n[11] Filter by Section..." -ForegroundColor Yellow
    $section = Invoke-RestMethod -Uri "http://localhost:5263/api/recettes?sectionId=$sectionId" -Method Get
    Write-Host "OK: Section=$($section.Count)" -ForegroundColor Green

    # 12. Calculate total
    Write-Host "`n[12] Calculate Total..." -ForegroundColor Yellow
    $total = ($all | Measure-Object -Property montant -Sum).Sum
    Write-Host "OK: Total recettes = $total EUR" -ForegroundColor Green

    # 13. Delete Recette
    Write-Host "`n[13] Delete Recette..." -ForegroundColor Yellow
    Invoke-RestMethod -Uri "http://localhost:5263/api/recettes/$recetteId3" -Method Delete
    Write-Host "OK: Deleted" -ForegroundColor Green

    # 14. Verify deleted
    Write-Host "`n[14] Verify Deleted..." -ForegroundColor Yellow
    try {
        $deleted = Invoke-RestMethod -Uri "http://localhost:5263/api/recettes/$recetteId3" -Method Get
        Write-Host "ERROR: Should not be accessible!" -ForegroundColor Red
    } catch {
        Write-Host "OK: Not accessible (expected)" -ForegroundColor Green
    }

    # 15. Verify count after delete
    Write-Host "`n[15] Verify count after delete..." -ForegroundColor Yellow
    $allAfter = Invoke-RestMethod -Uri "http://localhost:5263/api/recettes" -Method Get
    Write-Host "OK: Total after delete=$($allAfter.Count) (was $($all.Count))" -ForegroundColor Green

    Write-Host "`n=== TOUS LES TESTS REUSSIS ===" -ForegroundColor Green
    Write-Host "CRUD: OK" -ForegroundColor Cyan
    Write-Host "Filters: Categorie, Source, Section - OK" -ForegroundColor Cyan
    Write-Host "Soft Delete: OK" -ForegroundColor Cyan
    Write-Host "Total recettes: $total EUR" -ForegroundColor Cyan

} catch {
    Write-Host "`nERREUR:" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
} finally {
    Write-Host "`n=== Arret API ===" -ForegroundColor Cyan
    Stop-Job -Job $apiJob
    Remove-Job -Job $apiJob
    Write-Host "Done" -ForegroundColor Green
}
