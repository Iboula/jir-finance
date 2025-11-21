# Test script for Depenses API
# Make sure the API is running on http://localhost:5263

Write-Host "=== Test Depenses API ===" -ForegroundColor Cyan

# 1. Get a section ID
Write-Host "`n1. Getting sections..." -ForegroundColor Yellow
$sections = Invoke-RestMethod -Uri "http://localhost:5263/api/sections" -Method Get
$sectionId = $sections[0].id
Write-Host "Using Section ID: $sectionId" -ForegroundColor Green

# 2. Create a new depense (Brouillon)
Write-Host "`n2. Creating depense (Brouillon status)..." -ForegroundColor Yellow
$createBody = @{
    sectionId = $sectionId
    numero = "DEP-TEST-001"
    dateDepense = "2024-11-17T00:00:00Z"
    categorie = "Fournitures"
    description = "Test depense - achat de fournitures"
    montant = 150.50
    beneficiaire = "Fournisseur XYZ"
    modePaiement = "Virement"
    referenceFacture = "FACT-2024-001"
    observations = "Test creation"
} | ConvertTo-Json

$createResponse = Invoke-RestMethod -Uri "http://localhost:5263/api/depenses" -Method Post -Body $createBody -ContentType "application/json"
$depenseId = $createResponse.id
Write-Host "Created Depense ID: $depenseId" -ForegroundColor Green

# 3. Get the created depense
Write-Host "`n3. Getting depense by ID..." -ForegroundColor Yellow
$depense = Invoke-RestMethod -Uri "http://localhost:5263/api/depenses/$depenseId" -Method Get
Write-Host "Depense Numero: $($depense.numero), Statut: $($depense.statut), Montant: $($depense.montant)" -ForegroundColor Green

# 4. Update the depense (only works in Brouillon status)
Write-Host "`n4. Updating depense..." -ForegroundColor Yellow
$updateBody = @{
    id = $depenseId
    sectionId = $sectionId
    numero = "DEP-TEST-001"
    dateDepense = "2024-11-17T00:00:00Z"
    categorie = "Fournitures"
    description = "Test depense - UPDATED"
    montant = 200.75
    beneficiaire = "Fournisseur XYZ"
    modePaiement = "Virement"
    referenceFacture = "FACT-2024-001"
    observations = "Test update"
} | ConvertTo-Json

Invoke-RestMethod -Uri "http://localhost:5263/api/depenses/$depenseId" -Method Put -Body $updateBody -ContentType "application/json"
Write-Host "Depense updated successfully" -ForegroundColor Green

# 5. Valider the depense (Brouillon -> Valide)
Write-Host "`n5. Validating depense (Brouillon -> Valide)..." -ForegroundColor Yellow
$validerBody = @{
    id = $depenseId
    validePar = "John Doe"
    observations = "Validation OK"
} | ConvertTo-Json

Invoke-RestMethod -Uri "http://localhost:5263/api/depenses/$depenseId/valider" -Method Put -Body $validerBody -ContentType "application/json"
Write-Host "Depense validated successfully" -ForegroundColor Green

# 6. Get depense to verify status = Valide
Write-Host "`n6. Verifying status after validation..." -ForegroundColor Yellow
$depense = Invoke-RestMethod -Uri "http://localhost:5263/api/depenses/$depenseId" -Method Get
Write-Host "Statut: $($depense.statut), ValidePar: $($depense.validePar)" -ForegroundColor Green

# 7. Payer the depense (Valide -> Paye)
Write-Host "`n7. Paying depense (Valide -> Paye)..." -ForegroundColor Yellow
$payerBody = @{
    id = $depenseId
    datePaiement = "2024-11-17T10:00:00Z"
    modePaiement = "Virement bancaire"
    referenceFacture = "FACT-2024-001-PAYE"
    observations = "Paiement effectué"
} | ConvertTo-Json

Invoke-RestMethod -Uri "http://localhost:5263/api/depenses/$depenseId/payer" -Method Put -Body $payerBody -ContentType "application/json"
Write-Host "Depense paid successfully" -ForegroundColor Green

# 8. Get depense to verify status = Paye
Write-Host "`n8. Verifying status after payment..." -ForegroundColor Yellow
$depense = Invoke-RestMethod -Uri "http://localhost:5263/api/depenses/$depenseId" -Method Get
Write-Host "Statut: $($depense.statut), DatePaiement: $($depense.datePaiement)" -ForegroundColor Green

# 9. Test rejection workflow - create another depense
Write-Host "`n9. Testing rejection workflow - creating another depense..." -ForegroundColor Yellow
$createBody2 = @{
    sectionId = $sectionId
    numero = "DEP-TEST-002"
    dateDepense = "2024-11-17T00:00:00Z"
    categorie = "Services"
    description = "Test depense to reject"
    montant = 50.00
    beneficiaire = "Provider ABC"
    observations = "Will be rejected"
} | ConvertTo-Json

$createResponse2 = Invoke-RestMethod -Uri "http://localhost:5263/api/depenses" -Method Post -Body $createBody2 -ContentType "application/json"
$depenseId2 = $createResponse2.id
Write-Host "Created Depense ID: $depenseId2" -ForegroundColor Green

# 10. Rejeter the depense (Brouillon -> Rejete)
Write-Host "`n10. Rejecting depense (Brouillon -> Rejete)..." -ForegroundColor Yellow
$rejeterBody = @{
    id = $depenseId2
    motifRejet = "Budget insuffisant"
    rejetePar = "Jane Smith"
} | ConvertTo-Json

Invoke-RestMethod -Uri "http://localhost:5263/api/depenses/$depenseId2/rejeter" -Method Put -Body $rejeterBody -ContentType "application/json"
Write-Host "Depense rejected successfully" -ForegroundColor Green

# 11. Verify rejected status
Write-Host "`n11. Verifying rejected status..." -ForegroundColor Yellow
$depense2 = Invoke-RestMethod -Uri "http://localhost:5263/api/depenses/$depenseId2" -Method Get
Write-Host "Statut: $($depense2.statut), MotifRejet: $($depense2.motifRejet)" -ForegroundColor Green

# 12. List all depenses
Write-Host "`n12. Listing all depenses..." -ForegroundColor Yellow
$allDepenses = Invoke-RestMethod -Uri "http://localhost:5263/api/depenses" -Method Get
Write-Host "Total depenses: $($allDepenses.Count)" -ForegroundColor Green
foreach ($d in $allDepenses) {
    Write-Host "  - $($d.numero): $($d.statut) - $($d.montant) EUR" -ForegroundColor Cyan
}

# 13. Filter by status
Write-Host "`n13. Filtering depenses by status=Paye..." -ForegroundColor Yellow
$payeDepenses = Invoke-RestMethod -Uri "http://localhost:5263/api/depenses?statut=Paye" -Method Get
Write-Host "Depenses payées: $($payeDepenses.Count)" -ForegroundColor Green

# 14. Create another depense for delete test
Write-Host "`n14. Creating depense for delete test..." -ForegroundColor Yellow
$createBody3 = @{
    sectionId = $sectionId
    numero = "DEP-TEST-003"
    dateDepense = "2024-11-17T00:00:00Z"
    categorie = "Test"
    description = "To be deleted"
    montant = 10.00
    beneficiaire = "Test"
} | ConvertTo-Json

$createResponse3 = Invoke-RestMethod -Uri "http://localhost:5263/api/depenses" -Method Post -Body $createBody3 -ContentType "application/json"
$depenseId3 = $createResponse3.id
Write-Host "Created Depense ID: $depenseId3" -ForegroundColor Green

# 15. Delete the depense (soft delete, only works in Brouillon)
Write-Host "`n15. Deleting depense (soft delete)..." -ForegroundColor Yellow
Invoke-RestMethod -Uri "http://localhost:5263/api/depenses/$depenseId3" -Method Delete
Write-Host "Depense deleted successfully" -ForegroundColor Green

# 16. Try to get deleted depense (should fail)
Write-Host "`n16. Trying to get deleted depense (should fail)..." -ForegroundColor Yellow
try {
    $deleted = Invoke-RestMethod -Uri "http://localhost:5263/api/depenses/$depenseId3" -Method Get
    Write-Host "ERROR: Deleted depense should not be accessible!" -ForegroundColor Red
} catch {
    Write-Host "Correct: Deleted depense is not accessible" -ForegroundColor Green
}

Write-Host "`n=== All tests completed ===" -ForegroundColor Cyan
