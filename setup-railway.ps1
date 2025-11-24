# Configuration complete du deploiement Railway
Write-Host "=== Configuration Railway ===" -ForegroundColor Cyan

# Variables d'environnement a configurer manuellement dans Railway
$envVars = @'
# Copiez ces variables dans Railway Dashboard
# Project: https://railway.com/project/990b025d-826c-4ab9-9876-a5a595392926

=== SERVICE: API (jir-api) ===
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://0.0.0.0:$PORT
ConnectionStrings__DefaultConnection=${{Postgres.DATABASE_URL}}

=== SERVICE: Blazor (jir-blazor) ===
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://0.0.0.0:$PORT
ApiBaseUrl=${{jir-api.url}}

=== SETTINGS ===
API Port: 8080
Blazor Port: 3000
Health Check: /health
'@

Write-Host $envVars -ForegroundColor White

# Sauvegarder dans un fichier
$envVars | Out-File -FilePath "railway-env-vars.txt" -Encoding UTF8
Write-Host "`nVariables sauvegardees dans railway-env-vars.txt" -ForegroundColor Green

# Instructions
Write-Host "`nETAPES SUIVANTES:" -ForegroundColor Yellow
Write-Host "1. Allez sur le dashboard Railway (ouverture automatique)" -ForegroundColor White
Write-Host "2. Cliquez sur + New pour creer un service" -ForegroundColor White
Write-Host "3. Selectionnez GitHub Repo -> Iboula/jir-finance -> feature/docker-charts" -ForegroundColor White
Write-Host "4. Une fois cree, allez dans Variables et collez les variables ci-dessus" -ForegroundColor White
Write-Host "5. Dans Settings, configurez:" -ForegroundColor White
Write-Host "   - Dockerfile Path: src/JIR.WebAPI/Dockerfile" -ForegroundColor Gray
Write-Host "6. Railway deploiera automatiquement!" -ForegroundColor White
Write-Host ""

# Ouvrir le dashboard
$url = "https://railway.com/project/990b025d-826c-4ab9-9876-a5a595392926"
Start-Process $url
