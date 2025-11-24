# Script de déploiement Railway automatique
Write-Host "=== Déploiement JIR Finance sur Railway ===" -ForegroundColor Cyan
Write-Host ""

# 1. Lier le repo GitHub au projet
Write-Host "1. Liaison du repository GitHub..." -ForegroundColor Yellow
railway link 990b025d-826c-4ab9-9876-a5a595392926

# 2. Obtenir l'URL de la base de données PostgreSQL
Write-Host "`n2. Récupération des variables PostgreSQL..." -ForegroundColor Yellow
$dbUrl = railway variables --service Postgres | Select-String "DATABASE_URL" | ForEach-Object { $_.ToString().Split('=')[1].Trim() }
Write-Host "DATABASE_URL trouvée" -ForegroundColor Green

# 3. Créer le service API
Write-Host "`n3. Configuration du service API..." -ForegroundColor Yellow

# Définir les variables d'environnement pour l'API
$apiEnvVars = @"
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://0.0.0.0:8080
PORT=8080
ConnectionStrings__DefaultConnection=$dbUrl
"@

# Sauvegarder dans un fichier temporaire
$apiEnvVars | Out-File -FilePath ".railway-api-env" -Encoding UTF8

Write-Host "Variables d'environnement API configurées" -ForegroundColor Green

# 4. Créer le fichier de configuration Railway pour l'API
$railwayConfig = @"
{
  "build": {
    "builder": "DOCKERFILE",
    "dockerfilePath": "src/JIR.WebAPI/Dockerfile"
  },
  "deploy": {
    "startCommand": "dotnet /app/JIR.WebAPI.dll",
    "restartPolicyType": "ON_FAILURE",
    "restartPolicyMaxRetries": 10,
    "healthcheckPath": "/health",
    "healthcheckTimeout": 300
  }
}
"@

$railwayConfig | Out-File -FilePath ".railway-service.json" -Encoding UTF8

# 5. Déployer l'API
Write-Host "`n4. Déploiement de l'API..." -ForegroundColor Yellow
railway up --detach

Write-Host "`n✅ Déploiement lancé!" -ForegroundColor Green
Write-Host "`nPour suivre les logs:" -ForegroundColor Cyan
Write-Host "  railway logs" -ForegroundColor White
Write-Host "`nPour voir le statut:" -ForegroundColor Cyan
Write-Host "  railway status" -ForegroundColor White
Write-Host "`nDashboard: https://railway.com/project/990b025d-826c-4ab9-9876-a5a595392926" -ForegroundColor Cyan
