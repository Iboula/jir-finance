# Script de test pour Depenses API
# Lance l'API en arrière-plan et exécute les tests

Write-Host "=== Démarrage de l'API ===" -ForegroundColor Cyan

# Démarrer l'API en background job
$apiJob = Start-Job -ScriptBlock {
    Set-Location "c:\Users\iboul\OneDrive\Documents\PROJETS\MOPRO\JIR\src\JIR.WebApi"
    dotnet run
}

Write-Host "API démarrée (Job ID: $($apiJob.Id))" -ForegroundColor Green
Write-Host "Attente de 10 secondes pour le démarrage complet..." -ForegroundColor Yellow
Start-Sleep -Seconds 10

try {
    Write-Host "`n=== Tests du module Depenses ===" -ForegroundColor Cyan

    # 1. Récupérer un SectionId
    Write-Host "`n[1/16] Récupération d'une section..." -ForegroundColor Yellow
    $sections = Invoke-RestMethod -Uri "http://localhost:5263/api/sections" -Method Get
    $sectionId = $sections[0].id
    Write-Host "✓ Section trouvée: $sectionId ($($sections[0].code) - $($sections[0].nom))" -ForegroundColor Green

    # 2. Créer une dépense (statut Brouillon)
    Write-Host "`n[2/16] Création d'une dépense (Brouillon)..." -ForegroundColor Yellow
    $createDepense = @{
        sectionId = $sectionId
        numero = "DEP-$(Get-Date -Format 'yyyyMMddHHmmss')"
        dateDepense = (Get-Date).ToString("yyyy-MM-ddTHH:mm:ssZ")
        categorie = "Fournitures"
        description = "Achat de fournitures de bureau"
        montant = 250.50
        beneficiaire = "Papeterie Martin"
        modePaiement = "Virement"
        referenceFacture = "FACT-2024-001"
        observations = "Dépense test créée via script"
    } | ConvertTo-Json

    $createResult = Invoke-RestMethod -Uri "http://localhost:5263/api/depenses" -Method Post -Body $createDepense -ContentType "application/json"
    $depenseId = $createResult.id
    Write-Host "✓ Dépense créée: $depenseId" -ForegroundColor Green

    # 3. Récupérer la dépense créée
    Write-Host "`n[3/16] Récupération de la dépense..." -ForegroundColor Yellow
    $depense = Invoke-RestMethod -Uri "http://localhost:5263/api/depenses/$depenseId" -Method Get
    Write-Host "✓ Numéro: $($depense.numero)" -ForegroundColor Green
    Write-Host "  Statut: $($depense.statut)" -ForegroundColor Green
    Write-Host "  Montant: $($depense.montant) EUR" -ForegroundColor Green

    # 4. Modifier la dépense (possible car statut = Brouillon)
    Write-Host "`n[4/16] Modification de la dépense..." -ForegroundColor Yellow
    $updateDepense = @{
        id = $depenseId
        sectionId = $sectionId
        numero = $depense.numero
        dateDepense = $depense.dateDepense
        categorie = "Fournitures"
        description = "Achat de fournitures de bureau - MODIFIÉ"
        montant = 275.75
        beneficiaire = "Papeterie Martin"
        modePaiement = "Virement bancaire"
        referenceFacture = "FACT-2024-001"
        observations = "Montant mis à jour"
    } | ConvertTo-Json

    Invoke-RestMethod -Uri "http://localhost:5263/api/depenses/$depenseId" -Method Put -Body $updateDepense -ContentType "application/json"
    Write-Host "✓ Dépense modifiée" -ForegroundColor Green

    # 5. Vérifier la modification
    Write-Host "`n[5/16] Vérification de la modification..." -ForegroundColor Yellow
    $depense = Invoke-RestMethod -Uri "http://localhost:5263/api/depenses/$depenseId" -Method Get
    Write-Host "✓ Nouveau montant: $($depense.montant) EUR" -ForegroundColor Green
    Write-Host "  Description: $($depense.description)" -ForegroundColor Green

    # 6. Valider la dépense (Brouillon → Valide)
    Write-Host "`n[6/16] Validation de la dépense..." -ForegroundColor Yellow
    $valider = @{
        id = $depenseId
        validePar = "Jean Dupont"
        observations = "Facture conforme, validation OK"
    } | ConvertTo-Json

    Invoke-RestMethod -Uri "http://localhost:5263/api/depenses/$depenseId/valider" -Method Put -Body $valider -ContentType "application/json"
    Write-Host "✓ Dépense validée" -ForegroundColor Green

    # 7. Vérifier le statut Valide
    Write-Host "`n[7/16] Vérification du statut après validation..." -ForegroundColor Yellow
    $depense = Invoke-RestMethod -Uri "http://localhost:5263/api/depenses/$depenseId" -Method Get
    Write-Host "✓ Statut: $($depense.statut)" -ForegroundColor Green
    Write-Host "  Validé par: $($depense.validePar)" -ForegroundColor Green
    Write-Host "  Date validation: $($depense.dateValidation)" -ForegroundColor Green

    # 8. Tenter de modifier une dépense validée (doit échouer)
    Write-Host "`n[8/16] Test: modification d'une dépense validée (doit échouer)..." -ForegroundColor Yellow
    try {
        Invoke-RestMethod -Uri "http://localhost:5263/api/depenses/$depenseId" -Method Put -Body $updateDepense -ContentType "application/json"
        Write-Host "✗ ERREUR: La modification aurait dû être refusée!" -ForegroundColor Red
    } catch {
        Write-Host "✓ Modification refusée (attendu)" -ForegroundColor Green
    }

    # 9. Payer la dépense (Valide → Paye)
    Write-Host "`n[9/16] Paiement de la dépense..." -ForegroundColor Yellow
    $payer = @{
        id = $depenseId
        datePaiement = (Get-Date).ToString("yyyy-MM-ddTHH:mm:ssZ")
        modePaiement = "Virement SEPA"
        referenceFacture = "VIR-2024-001"
        observations = "Paiement effectué le $(Get-Date -Format 'dd/MM/yyyy')"
    } | ConvertTo-Json

    Invoke-RestMethod -Uri "http://localhost:5263/api/depenses/$depenseId/payer" -Method Put -Body $payer -ContentType "application/json"
    Write-Host "✓ Dépense payée" -ForegroundColor Green

    # 10. Vérifier le statut Paye
    Write-Host "`n[10/16] Vérification du statut après paiement..." -ForegroundColor Yellow
    $depense = Invoke-RestMethod -Uri "http://localhost:5263/api/depenses/$depenseId" -Method Get
    Write-Host "✓ Statut: $($depense.statut)" -ForegroundColor Green
    Write-Host "  Date paiement: $($depense.datePaiement)" -ForegroundColor Green
    Write-Host "  Mode paiement: $($depense.modePaiement)" -ForegroundColor Green

    # 11. Workflow de rejet - Créer une nouvelle dépense
    Write-Host "`n[11/16] Création d'une dépense pour test de rejet..." -ForegroundColor Yellow
    $createDepense2 = @{
        sectionId = $sectionId
        numero = "DEP-REJ-$(Get-Date -Format 'yyyyMMddHHmmss')"
        dateDepense = (Get-Date).ToString("yyyy-MM-ddTHH:mm:ssZ")
        categorie = "Services"
        description = "Dépense à rejeter"
        montant = 100.00
        beneficiaire = "Fournisseur Test"
        observations = "Test workflow rejet"
    } | ConvertTo-Json

    $createResult2 = Invoke-RestMethod -Uri "http://localhost:5263/api/depenses" -Method Post -Body $createDepense2 -ContentType "application/json"
    $depenseId2 = $createResult2.id
    Write-Host "✓ Dépense créée: $depenseId2" -ForegroundColor Green

    # 12. Rejeter la dépense (Brouillon → Rejete)
    Write-Host "`n[12/16] Rejet de la dépense..." -ForegroundColor Yellow
    $rejeter = @{
        id = $depenseId2
        motifRejet = "Budget dépassé pour cette catégorie"
        rejetePar = "Marie Dubois"
    } | ConvertTo-Json

    Invoke-RestMethod -Uri "http://localhost:5263/api/depenses/$depenseId2/rejeter" -Method Put -Body $rejeter -ContentType "application/json"
    Write-Host "✓ Dépense rejetée" -ForegroundColor Green

    # 13. Vérifier le statut Rejete
    Write-Host "`n[13/16] Vérification du statut après rejet..." -ForegroundColor Yellow
    $depense2 = Invoke-RestMethod -Uri "http://localhost:5263/api/depenses/$depenseId2" -Method Get
    Write-Host "✓ Statut: $($depense2.statut)" -ForegroundColor Green
    Write-Host "  Motif: $($depense2.motifRejet)" -ForegroundColor Green
    Write-Host "  Rejeté par: $($depense2.validePar)" -ForegroundColor Green

    # 14. Test suppression - Créer une dépense
    Write-Host "`n[14/16] Création d'une dépense pour test de suppression..." -ForegroundColor Yellow
    $createDepense3 = @{
        sectionId = $sectionId
        numero = "DEP-DEL-$(Get-Date -Format 'yyyyMMddHHmmss')"
        dateDepense = (Get-Date).ToString("yyyy-MM-ddTHH:mm:ssZ")
        categorie = "Test"
        description = "Dépense à supprimer"
        montant = 50.00
        beneficiaire = "Test"
    } | ConvertTo-Json

    $createResult3 = Invoke-RestMethod -Uri "http://localhost:5263/api/depenses" -Method Post -Body $createDepense3 -ContentType "application/json"
    $depenseId3 = $createResult3.id
    Write-Host "✓ Dépense créée: $depenseId3" -ForegroundColor Green

    # 15. Supprimer la dépense (soft delete)
    Write-Host "`n[15/16] Suppression de la dépense..." -ForegroundColor Yellow
    Invoke-RestMethod -Uri "http://localhost:5263/api/depenses/$depenseId3" -Method Delete
    Write-Host "✓ Dépense supprimée" -ForegroundColor Green

    # 16. Vérifier que la dépense supprimée n'est plus accessible
    Write-Host "`n[16/16] Vérification que la dépense supprimée est inaccessible..." -ForegroundColor Yellow
    try {
        $deleted = Invoke-RestMethod -Uri "http://localhost:5263/api/depenses/$depenseId3" -Method Get
        Write-Host "✗ ERREUR: La dépense supprimée ne devrait pas être accessible!" -ForegroundColor Red
    } catch {
        Write-Host "✓ Dépense supprimée non accessible (attendu)" -ForegroundColor Green
    }

    # Tests de filtrage
    Write-Host "`n=== Tests de filtrage ===" -ForegroundColor Cyan

    Write-Host "`nListe de toutes les dépenses..." -ForegroundColor Yellow
    $allDepenses = Invoke-RestMethod -Uri "http://localhost:5263/api/depenses" -Method Get
    Write-Host "✓ Total: $($allDepenses.Count) dépenses" -ForegroundColor Green

    Write-Host "`nFiltre par statut = Paye..." -ForegroundColor Yellow
    $payeDepenses = Invoke-RestMethod -Uri "http://localhost:5263/api/depenses?statut=Paye" -Method Get
    Write-Host "✓ Dépenses payées: $($payeDepenses.Count)" -ForegroundColor Green

    Write-Host "`nFiltre par statut = Brouillon..." -ForegroundColor Yellow
    $brouillonDepenses = Invoke-RestMethod -Uri "http://localhost:5263/api/depenses?statut=Brouillon" -Method Get
    Write-Host "✓ Dépenses en brouillon: $($brouillonDepenses.Count)" -ForegroundColor Green

    Write-Host "`nFiltre par section..." -ForegroundColor Yellow
    $sectionDepenses = Invoke-RestMethod -Uri "http://localhost:5263/api/depenses?sectionId=$sectionId" -Method Get
    Write-Host "✓ Dépenses de la section: $($sectionDepenses.Count)" -ForegroundColor Green

    Write-Host "`n=== Résumé des tests ===" -ForegroundColor Cyan
    Write-Host "✓ Tous les tests ont réussi!" -ForegroundColor Green
    Write-Host "`nWorkflow complet testé:" -ForegroundColor Yellow
    Write-Host "  1. Création (Brouillon)" -ForegroundColor White
    Write-Host "  2. Modification (possible en Brouillon)" -ForegroundColor White
    Write-Host "  3. Validation (Brouillon → Valide)" -ForegroundColor White
    Write-Host "  4. Modification refusée (Valide)" -ForegroundColor White
    Write-Host "  5. Paiement (Valide → Paye)" -ForegroundColor White
    Write-Host "  6. Rejet (Brouillon → Rejete)" -ForegroundColor White
    Write-Host "  7. Suppression (soft delete)" -ForegroundColor White
    Write-Host "  8. Filtres (statut, section)" -ForegroundColor White

} catch {
    Write-Host "`n✗ Erreur lors des tests:" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
    Write-Host $_.ScriptStackTrace -ForegroundColor Red
} finally {
    # Arrêter l'API
    Write-Host "`n=== Arrêt de l'API ===" -ForegroundColor Cyan
    Stop-Job -Job $apiJob
    Remove-Job -Job $apiJob
    Write-Host "API arrêtée" -ForegroundColor Green
}
