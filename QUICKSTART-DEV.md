# 🚀 Quick Start Guide - JIR Financial Management System

## ⚡ Démarrage Rapide (5 minutes)

### Option 1: Sans PostgreSQL Local (Recommandé pour débuter)

```powershell
# 1. Cloner et naviguer
cd C:\Users\iboul\OneDrive\Documents\PROJETS\MOPRO\JIR

# 2. Restaurer les packages
dotnet restore

# 3. Démarrer l'API (utilisera SQLite en mémoire pour les tests)
cd src\JIR.WebAPI
dotnet run

# 4. Ouvrir Swagger dans votre navigateur
start https://localhost:7000/swagger
```

### Option 2: Avec PostgreSQL Local

#### Prérequis
- PostgreSQL 16 installé et en cours d'exécution
- Mot de passe postgres configuré

#### Étapes

```powershell
# 1. Créer la base de données et les tables
psql -U postgres -f scripts\sql\01-create-database.sql
psql -U postgres -d jir_finance -f scripts\sql\02-create-tables.sql
psql -U postgres -d jir_finance -f scripts\sql\03-create-indexes.sql
psql -U postgres -d jir_finance -f scripts\sql\04-create-triggers.sql
psql -U postgres -d jir_finance -f scripts\sql\05-create-views.sql
psql -U postgres -d jir_finance -f scripts\sql\06-seed-data.sql

# 2. Configurer la chaîne de connexion
# Éditer src\JIR.WebAPI\appsettings.json
# Modifier "Password=postgres" avec votre mot de passe

# 3. Démarrer l'API
cd src\JIR.WebAPI
dotnet run

# 4. Ouvrir Swagger
start https://localhost:7000/swagger
```

### Option 3: Via EF Core Migrations (Alternative)

```powershell
# 1. Configurer appsettings.json avec vos credentials PostgreSQL

# 2. Appliquer les migrations
dotnet ef database update --project src\JIR.Infrastructure --startup-project src\JIR.WebAPI

# 3. Insérer les données de test manuellement ou via script

# 4. Démarrer l'API
cd src\JIR.WebAPI
dotnet run
```

## 🧪 Tester l'API

### Via Swagger UI

1. Ouvrir `https://localhost:7000/swagger`
2. Essayer l'endpoint `GET /api/sections`
3. Vous devriez voir 4 sections (Administration, Finances, Logistique, Formation)

### Via curl

```powershell
# Récupérer toutes les sections
curl -k https://localhost:7000/api/sections

# Récupérer une section par ID
curl -k https://localhost:7000/api/sections/GUID-ICI

# Créer une nouvelle section
curl -k -X POST https://localhost:7000/api/sections `
  -H "Content-Type: application/json" `
  -d '{\"code\":\"SEC005\",\"nom\":\"Marketing\",\"budgetAnnuel\":150000}'
```

### Via PowerShell

```powershell
# Récupérer toutes les sections
Invoke-RestMethod -Uri "https://localhost:7000/api/sections" -SkipCertificateCheck

# Créer une section
$body = @{
    code = "SEC005"
    nom = "Marketing"
    budgetAnnuel = 150000
} | ConvertTo-Json

Invoke-RestMethod -Uri "https://localhost:7000/api/sections" `
    -Method Post `
    -Body $body `
    -ContentType "application/json" `
    -SkipCertificateCheck
```

## 📊 Données de Test Disponibles

Une fois le script `06-seed-data.sql` exécuté, vous aurez :

### 👥 Utilisateurs
- 6 comptes utilisateurs (admin, 2 managers, 2 users, 1 viewer)
- Mot de passe pour tous : `Password123!`

### 🏢 Sections
- 4 sections organisationnelles
- Budgets annuels configurés
- Responsables assignés

### 💰 Données Financières
- 12+ cotisations sur 2 mois
- 7 dépenses (divers statuts de workflow)
- 5 recettes

### 🤝 Partenaires
- 5 partenaires (fournisseurs, sponsors, clients)

### 📦 Magasins
- 2 magasins
- 10 articles en stock
- Alertes de stock bas configurées

## 🔍 Vérification de l'Installation

### Checklist
- [ ] Solution build sans erreurs: `dotnet build`
- [ ] Base de données créée (si PostgreSQL)
- [ ] API démarre: `dotnet run` dans WebAPI
- [ ] Swagger accessible: https://localhost:7000/swagger
- [ ] GET /api/sections retourne des données

### Commandes de Diagnostic

```powershell
# Vérifier la version de .NET
dotnet --version
# Doit afficher: 9.0.x

# Vérifier les packages installés
dotnet list package

# Vérifier la structure de la base de données (PostgreSQL)
psql -U postgres -d jir_finance -c "\dt"

# Voir les migrations EF Core
dotnet ef migrations list --project src\JIR.Infrastructure --startup-project src\JIR.WebAPI

# Build avec détails
dotnet build --verbosity detailed
```

## 🐛 Dépannage Courant

### Erreur: "Database does not exist"
```powershell
# Créer la base de données
psql -U postgres -f scripts\sql\01-create-database.sql
```

### Erreur: "Connection refused"
```powershell
# Vérifier que PostgreSQL est démarré
# Windows: Services > PostgreSQL
# Ou via commande
pg_isready -U postgres
```

### Erreur: Build échoue
```powershell
# Nettoyer et rebuild
dotnet clean
dotnet restore
dotnet build
```

### Port déjà utilisé
```powershell
# Modifier le port dans src\JIR.WebAPI\Properties\launchSettings.json
# Ou arrêter le processus utilisant le port
netstat -ano | findstr :7000
taskkill /PID [PID] /F
```

## 📚 Prochaines Étapes

### 1. Explorer l'API
- Tester tous les endpoints Sections via Swagger
- Créer/Modifier/Supprimer des sections
- Observer la validation automatique

### 2. Développement
- Implémenter les modules restants (Cotisations, Dépenses, Recettes, etc.)
- Ajouter l'authentification JWT
- Créer le frontend Blazor

### 3. Tests
```powershell
# Exécuter les tests unitaires
dotnet test
```

### 4. Docker (Optionnel)
```powershell
# Build et run avec Docker
docker-compose up -d

# Voir les logs
docker-compose logs -f api
```

## 💡 Conseils de Développement

### VS Code Extensions Recommandées
- C# Dev Kit
- .NET Runtime Install Tool
- REST Client
- PostgreSQL Explorer
- Docker

### Raccourcis Utiles
```powershell
# Watch mode (rebuild automatique)
dotnet watch run --project src\JIR.WebAPI

# Format du code
dotnet format

# Voir les références de packages
dotnet list package --outdated
```

## 📞 Support

- Consulter le README.md principal pour la documentation complète
- Vérifier les agents/ pour les configurations MCP
- Consulter CAHIER-DES-CHARGES.md pour les spécifications

---

**Temps estimé du quick start**: 5-10 minutes  
**Difficulté**: ⭐⭐☆☆☆ Débutant  
**Dernière mise à jour**: Novembre 2025
