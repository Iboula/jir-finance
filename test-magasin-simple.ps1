# Test POST Magasin
Write-Host "Test POST magasin..." -ForegroundColor Cyan

try {
    $body = @{
        code = "MAG-TEST"
        nom = "Magasin Test"
        localisation = "Test"
    } | ConvertTo-Json

    Write-Host "Body: $body" -ForegroundColor Gray
    
    $result = Invoke-RestMethod -Uri "http://localhost:5001/api/magasins" -Method Post -Body $body -ContentType "application/json" -UseBasicParsing -ErrorAction Stop
    
    Write-Host "[OK] Magasin cree: $($result | ConvertTo-Json)" -ForegroundColor Green
} catch {
    Write-Host "[FAIL] Erreur: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "StatusCode: $($_.Exception.Response.StatusCode.Value__)" -ForegroundColor Red
    Write-Host "ErrorDetails: $($_.ErrorDetails.Message)" -ForegroundColor Red
    
    # Essayer de lire le corps de la reponse
    try {
        $reader = [System.IO.StreamReader]::new($_.Exception.Response.GetResponseStream())
        $reader.BaseStream.Position = 0
        $reader.DiscardBufferedData()
        $responseBody = $reader.ReadToEnd()
        Write-Host "Response Body: $responseBody" -ForegroundColor Red
    } catch {
        # Ignore
    }
}
