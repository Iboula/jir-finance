# Script pour initialiser la base de données Railway
param(
    [Parameter(Mandatory=$false)]
    [string]$ApiUrl
)

if ([string]::IsNullOrEmpty($ApiUrl)) {
    Write-Host "Entrez l'URL de votre API Railway:" -ForegroundColor Cyan
    Write-Host "Exemple: https://jir-api-production-XXXX.up.railway.app" -ForegroundColor Gray
    $ApiUrl = Read-Host "URL"
}

# Retirer le trailing slash si présent
$ApiUrl = $ApiUrl.TrimEnd('/')

Write-Host "`n=== Initialisation de la base de donnees ===" -ForegroundColor Cyan
Write-Host ""

# 1. Vérifier la santé de l'API
Write-Host "1. Verification de la sante de l'API..." -ForegroundColor Yellow
try {
    $health = Invoke-RestMethod -Uri "$ApiUrl/health" -Method Get
    Write-Host "   Status: $($health.status)" -ForegroundColor Green
    Write-Host "   Timestamp: $($health.timestamp)" -ForegroundColor Gray
}
catch {
    Write-Host "   Erreur: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "   Verification que l'URL est correcte et que l'API est deployee" -ForegroundColor Yellow
    exit 1
}

# 2. Vérifier le statut de la base de données
Write-Host "`n2. Verification du statut de la base de donnees..." -ForegroundColor Yellow
try {
    $status = Invoke-RestMethod -Uri "$ApiUrl/api/seed/status" -Method Get
    Write-Host "   Connexion: $($status.connected)" -ForegroundColor $(if($status.connected){"Green"}else{"Red"})
    Write-Host "   Migrations appliquees: $($status.appliedMigrationsCount)" -ForegroundColor Gray
    Write-Host "   Migrations en attente: $($status.pendingMigrationsCount)" -ForegroundColor Gray
    Write-Host "   Base de donnees seedee: $($status.isSeeded)" -ForegroundColor $(if($status.isSeeded){"Green"}else{"Yellow"})
    Write-Host "   Comptes: $($status.accountsCount)" -ForegroundColor Gray
    Write-Host "   Transactions: $($status.transactionsCount)" -ForegroundColor Gray
}
catch {
    Write-Host "   Erreur: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

# 3. Demander confirmation pour initialiser
if ($status.isSeeded -and $status.pendingMigrationsCount -eq 0) {
    Write-Host "`nLa base de donnees est deja initialisee!" -ForegroundColor Green
    $confirm = Read-Host "`nVoulez-vous re-initialiser quand meme? (o/N)"
    if ($confirm -ne "o" -and $confirm -ne "O") {
        Write-Host "Operation annulee" -ForegroundColor Yellow
        exit 0
    }
}

Write-Host "`n3. Initialisation de la base de donnees..." -ForegroundColor Yellow
Write-Host "   (Migrations + Seed)" -ForegroundColor Gray

try {
    $result = Invoke-RestMethod -Uri "$ApiUrl/api/seed/initialize" -Method Post
    
    if ($result.success) {
        Write-Host "`n   Succes!" -ForegroundColor Green
        Write-Host "   Message: $($result.message)" -ForegroundColor Gray
        Write-Host "   Migrations appliquees: $($result.migrationsApplied)" -ForegroundColor Gray
        Write-Host "   Timestamp: $($result.timestamp)" -ForegroundColor Gray
    }
    else {
        Write-Host "`n   Echec!" -ForegroundColor Red
        Write-Host "   Erreur: $($result.error)" -ForegroundColor Red
    }
}
catch {
    Write-Host "`n   Erreur: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "   Reponse: $($_.ErrorDetails.Message)" -ForegroundColor Red
    exit 1
}

# 4. Vérifier à nouveau
Write-Host "`n4. Verification finale..." -ForegroundColor Yellow
try {
    $finalStatus = Invoke-RestMethod -Uri "$ApiUrl/api/seed/status" -Method Get
    Write-Host "   Comptes: $($finalStatus.accountsCount)" -ForegroundColor Green
    Write-Host "   Transactions: $($finalStatus.transactionsCount)" -ForegroundColor Green
}
catch {
    Write-Host "   Erreur: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "`n=== Initialisation terminee ===" -ForegroundColor Green
Write-Host "`nAccedez a Swagger: $ApiUrl/swagger" -ForegroundColor Cyan
