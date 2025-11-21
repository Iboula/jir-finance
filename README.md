# 🏦 JIR Financial Management System

Système de gestion financière moderne construit avec **.NET 9**, **Blazor WebAssembly**, et **PostgreSQL**.

## 📋 Table des Matières

- [Technologies](#-technologies)
- [Architecture](#-architecture)
- [Prérequis](#-prérequis)
- [Installation](#-installation)
- [Utilisation](#-utilisation)
- [Fonctionnalités](#-fonctionnalités)
- [Structure du Projet](#-structure-du-projet)
- [API Documentation](#-api-documentation)
- [Tests](#-tests)
- [Déploiement](#-déploiement)

## 🚀 Technologies

### Backend
- **.NET 9** - Framework backend moderne
- **ASP.NET Core Web API** - RESTful API
- **Entity Framework Core 9** - ORM
- **PostgreSQL 16** - Base de données relationnelle
- **MediatR** - Pattern CQRS et Mediator
- **AutoMapper** - Mapping objet-objet
- **FluentValidation** - Validation des données
- **Swashbuckle** - Documentation API (Swagger/OpenAPI)

### Frontend (À venir)
- **Blazor WebAssembly** - SPA Framework
- **Radzen Blazor Components** - UI Components
- **QuestPDF** - Génération de PDF

### Infrastructure
- **Docker & Docker Compose** - Conteneurisation
- **NGINX** - Reverse proxy
- **xUnit** - Tests unitaires
- **Moq** - Mocking framework

## 🏗️ Architecture

Le projet suit les principes de **Clean Architecture** avec une séparation claire des responsabilités :

```
JIR/
├── src/
│   ├── JIR.Domain/           # Entités métier, enums
│   ├── JIR.Application/      # Logique métier, CQRS
│   ├── JIR.Infrastructure/   # Accès données, services
│   └── JIR.WebAPI/           # API REST, contrôleurs
├── tests/
│   └── JIR.Tests/            # Tests unitaires
├── scripts/
│   └── sql/                  # Scripts SQL PostgreSQL
└── agents/                   # Configurations MCP agents
```

### Patterns Utilisés
- **CQRS** (Command Query Responsibility Segregation)
- **Mediator Pattern** via MediatR
- **Repository Pattern** via EF Core DbContext
- **Unit of Work** via DbContext
- **Dependency Injection**
- **Pipeline Behavior** (Validation, Error Handling)

## 📦 Prérequis

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [PostgreSQL 16](https://www.postgresql.org/download/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (optionnel)
- [VS Code](https://code.visualstudio.com/) ou [Visual Studio 2022](https://visualstudio.microsoft.com/)

## ⚙️ Installation

### 1. Cloner le Repository

```bash
git clone https://github.com/votre-repo/jir-finance.git
cd jir-finance
```

### 2. Configuration de la Base de Données

#### Option A: PostgreSQL Local

```bash
# Exécuter les scripts SQL dans l'ordre
psql -U postgres -f scripts/sql/01-create-database.sql
psql -U postgres -d jir_finance -f scripts/sql/02-create-tables.sql
psql -U postgres -d jir_finance -f scripts/sql/03-create-indexes.sql
psql -U postgres -d jir_finance -f scripts/sql/04-create-triggers.sql
psql -U postgres -d jir_finance -f scripts/sql/05-create-views.sql
psql -U postgres -d jir_finance -f scripts/sql/06-seed-data.sql
```

#### Option B: Via EF Core Migrations

```bash
# Appliquer les migrations
dotnet ef database update --project src/JIR.Infrastructure --startup-project src/JIR.WebAPI
```

### 3. Configuration de l'API

Mettre à jour `src/JIR.WebAPI/appsettings.json` :

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=jir_finance;Username=postgres;Password=VOTRE_MOT_DE_PASSE"
  }
}
```

### 4. Restaurer les Dépendances

```bash
dotnet restore
```

### 5. Build du Projet

```bash
dotnet build
```

## 🎯 Utilisation

### Démarrer l'API

```bash
cd src/JIR.WebAPI
dotnet run
```

L'API sera disponible sur :
- **HTTPS**: `https://localhost:7000`
- **HTTP**: `http://localhost:5000`
- **Swagger UI**: `https://localhost:7000/swagger`

### Comptes de Test

Après avoir exécuté le script de seed data :

| Username  | Mot de passe  | Rôle    | Section        |
|-----------|---------------|---------|----------------|
| admin     | Password123!  | Admin   | -              |
| manager1  | Password123!  | Manager | Administration |
| manager2  | Password123!  | Manager | Finances       |
| user1     | Password123!  | User    | Administration |
| user2     | Password123!  | User    | Finances       |
| viewer    | Password123!  | Viewer  | -              |

## ✨ Fonctionnalités

### 📊 Module Sections
- ✅ CRUD complet des sections organisationnelles
- ✅ Gestion des budgets annuels
- ✅ Attribution des responsables

### 💰 Module Cotisations
- 📝 Enregistrement des cotisations mensuelles
- 📄 Génération automatique de reçus
- 📈 Statistiques par période

### 💳 Module Dépenses
- 🔄 Workflow de validation à 2 niveaux
- 📎 Pièces justificatives
- 🏷️ Catégorisation
- 📊 Suivi des statuts

### 💵 Module Recettes
- ✅ Enregistrement multi-sources
- 📄 Numérotation automatique
- 📈 Rapports financiers

### 🤝 Module Partenaires
- ✅ Gestion fournisseurs/sponsors/clients
- 📞 Contacts et coordonnées
- 🏢 Classification par type

### 📦 Module Magasins
- 📊 Gestion multi-entrepôts
- 🔔 Alertes stock bas
- 📈 Historique des mouvements
- ⚖️ Mise à jour automatique des stocks

## 📁 Structure du Projet

### Domain Layer (`JIR.Domain`)
```
Domain/
├── Common/
│   ├── BaseEntity.cs
│   └── AuditableEntity.cs
├── Entities/
│   ├── User.cs
│   ├── Section.cs
│   ├── Cotisation.cs
│   ├── Depense.cs
│   ├── Recette.cs
│   ├── Partenaire.cs
│   ├── Magasin.cs
│   ├── ArticleMagasin.cs
│   └── MouvementStock.cs
└── Enums/
    ├── UserRole.cs
    ├── StatutWorkflow.cs
    ├── TypePartenaire.cs
    └── TypeMouvement.cs
```

### Application Layer (`JIR.Application`)
```
Application/
├── Common/
│   ├── Interfaces/
│   │   ├── IApplicationDbContext.cs
│   │   └── ICurrentUserService.cs
│   ├── Mappings/
│   │   └── MappingProfile.cs
│   └── Behaviors/
│       ├── ValidationBehavior.cs
│       └── UnhandledExceptionBehavior.cs
├── Sections/
│   ├── Commands/
│   │   ├── CreateSection/
│   │   ├── UpdateSection/
│   │   └── DeleteSection/
│   └── Queries/
│       ├── GetSections/
│       └── GetSectionById/
└── DependencyInjection.cs
```

### Infrastructure Layer (`JIR.Infrastructure`)
```
Infrastructure/
├── Persistence/
│   ├── ApplicationDbContext.cs
│   ├── Configurations/
│   │   ├── UserConfiguration.cs
│   │   ├── SectionConfiguration.cs
│   │   ├── DepenseConfiguration.cs
│   │   ├── MagasinConfiguration.cs
│   │   └── ArticleMagasinConfiguration.cs
│   └── Migrations/
├── Services/
│   └── CurrentUserService.cs
└── DependencyInjection.cs
```

### WebAPI Layer (`JIR.WebAPI`)
```
WebAPI/
├── Controllers/
│   └── SectionsController.cs
├── Program.cs
└── appsettings.json
```

## 📖 API Documentation

Une fois l'API démarrée, la documentation Swagger est accessible à :

```
https://localhost:7000/swagger
```

### Endpoints Principaux

#### Sections
- `GET /api/sections` - Liste toutes les sections
- `GET /api/sections/{id}` - Détails d'une section
- `POST /api/sections` - Créer une section
- `PUT /api/sections/{id}` - Mettre à jour une section
- `DELETE /api/sections/{id}` - Supprimer une section (soft delete)

## 🧪 Tests

```bash
# Exécuter tous les tests
dotnet test

# Avec couverture de code
dotnet test /p:CollectCoverage=true /p:CoverageReportsFormat=opencover

# Tests d'un projet spécifique
dotnet test tests/JIR.Tests/JIR.Tests.csproj
```

## 🐳 Déploiement

### Avec Docker Compose

```bash
# Build et démarrage
docker-compose up -d

# Voir les logs
docker-compose logs -f

# Arrêt
docker-compose down
```

### Services Exposés
- **API**: `http://localhost:5000`
- **Blazor**: `http://localhost:5001`
- **PostgreSQL**: `localhost:5432`
- **PgAdmin**: `http://localhost:5050`

## 📝 Scripts Utiles

```bash
# Créer une nouvelle migration
dotnet ef migrations add NomMigration --project src/JIR.Infrastructure --startup-project src/JIR.WebAPI

# Appliquer les migrations
dotnet ef database update --project src/JIR.Infrastructure --startup-project src/JIR.WebAPI

# Supprimer la dernière migration
dotnet ef migrations remove --project src/JIR.Infrastructure --startup-project src/JIR.WebAPI

# Générer un script SQL
dotnet ef migrations script --project src/JIR.Infrastructure --startup-project src/JIR.WebAPI --output migration.sql
```

## 🤝 Contribution

1. Fork le projet
2. Créer une branche feature (`git checkout -b feature/AmazingFeature`)
3. Commit les changements (`git commit -m 'Add some AmazingFeature'`)
4. Push vers la branche (`git push origin feature/AmazingFeature`)
5. Ouvrir une Pull Request

## 📄 Licence

Ce projet est sous licence MIT. Voir le fichier `LICENSE` pour plus de détails.

## 👥 Équipe

- **Backend Architect** - Architecture .NET, CQRS
- **Database Engineer** - PostgreSQL, optimisation
- **Frontend Developer** - Blazor, UI/UX
- **DevOps Engineer** - Docker, CI/CD
- **PDF Automation** - QuestPDF, documents

## 🔗 Liens Utiles

- [Documentation .NET 9](https://docs.microsoft.com/en-us/dotnet/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [MediatR](https://github.com/jbogard/MediatR)
- [FluentValidation](https://fluentvalidation.net/)
- [AutoMapper](https://automapper.org/)
- [Blazor](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)
- [PostgreSQL](https://www.postgresql.org/docs/)

---

**Status**: 🚧 En développement actif | **Version**: 1.0.0-alpha | **Date**: Novembre 2025
