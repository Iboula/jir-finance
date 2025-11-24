# Déploiement Railway via GitHub
Write-Host "=== Déploiement JIR Finance sur Railway ===" -ForegroundColor Cyan
Write-Host ""

Write-Host "📋 Configuration requise dans le dashboard Railway:" -ForegroundColor Yellow
Write-Host ""
Write-Host "1. Ouvrez: https://railway.com/project/990b025d-826c-4ab9-9876-a5a595392926" -ForegroundColor Cyan
Write-Host ""
Write-Host "2. Créez un nouveau service (+ New):" -ForegroundColor White
Write-Host "   - Cliquez sur '+ New'" -ForegroundColor Gray
Write-Host "   - Sélectionnez 'GitHub Repo'" -ForegroundColor Gray
Write-Host "   - Choisissez 'Iboula/jir-finance'" -ForegroundColor Gray
Write-Host "   - Branche: 'feature/docker-charts'" -ForegroundColor Gray
Write-Host ""

Write-Host "3. Configurez les variables d'environnement:" -ForegroundColor White
Write-Host "   ASPNETCORE_ENVIRONMENT=Production" -ForegroundColor Gray
Write-Host "   ASPNETCORE_URLS=http://0.0.0.0:8080" -ForegroundColor Gray
Write-Host "   PORT=8080" -ForegroundColor Gray
Write-Host '   ConnectionStrings__DefaultConnection=${{Postgres.DATABASE_URL}}' -ForegroundColor Gray
Write-Host ""

Write-Host "4. Dans Settings:" -ForegroundColor White
Write-Host "   - Build Command: (laissez vide, Dockerfile sera utilisé)" -ForegroundColor Gray
Write-Host "   - Start Command: dotnet /app/JIR.WebAPI.dll" -ForegroundColor Gray
Write-Host "   - Custom Domain: (optionnel)" -ForegroundColor Gray
Write-Host ""

Write-Host "5. Railway va automatiquement:" -ForegroundColor White
Write-Host "   ✓ Détecter le Dockerfile" -ForegroundColor Green
Write-Host "   ✓ Construire l'image Docker" -ForegroundColor Green
Write-Host "   ✓ Déployer l'application" -ForegroundColor Green
Write-Host "   ✓ Générer une URL publique" -ForegroundColor Green
Write-Host ""

Write-Host "⏳ Le déploiement prend environ 3-5 minutes..." -ForegroundColor Yellow
Write-Host ""

# Ouvrir le dashboard
Start-Process "https://railway.com/project/990b025d-826c-4ab9-9876-a5a595392926"

Write-Host "✅ Dashboard ouvert dans votre navigateur!" -ForegroundColor Green
