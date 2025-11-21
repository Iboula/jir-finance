# JIR - Gestion de Stock avec Docker

## 🐳 Démarrage avec Docker

### Prérequis
- Docker Desktop installé et démarré
- Docker Compose (inclus avec Docker Desktop)

### Lancer l'application

```bash
# Depuis la racine du projet
docker-compose up --build
```

Cette commande va :
1. Créer un conteneur PostgreSQL avec la base de données
2. Builder et démarrer l'API backend sur le port 5001
3. Builder et démarrer l'application Blazor sur le port 5002

### Accéder à l'application

- **Frontend Blazor**: http://localhost:5002
- **API Backend**: http://localhost:5001/swagger
- **Base de données PostgreSQL**: localhost:5432

### Arrêter l'application

```bash
# Arrêter les conteneurs
docker-compose down

# Arrêter et supprimer les volumes (⚠️ supprime les données)
docker-compose down -v
```

### Commandes utiles

```bash
# Voir les logs en temps réel
docker-compose logs -f

# Voir les logs d'un service spécifique
docker-compose logs -f api
docker-compose logs -f blazor
docker-compose logs -f postgres

# Rebuilder sans cache
docker-compose build --no-cache

# Lister les conteneurs actifs
docker-compose ps

# Exécuter les migrations de la base de données
docker-compose exec api dotnet ef database update
```

## 📦 Architecture des conteneurs

```
┌─────────────────┐
│  Blazor (5002)  │
│   Frontend      │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│   API (5001)    │
│    Backend      │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ PostgreSQL      │
│   (5432)        │
└─────────────────┘
```

## 🔧 Configuration

### Variables d'environnement

Les variables suivantes peuvent être modifiées dans `docker-compose.yml` :

**PostgreSQL:**
- `POSTGRES_USER`: jir_user
- `POSTGRES_PASSWORD`: jir_password
- `POSTGRES_DB`: jir_db

**API:**
- `ConnectionStrings__DefaultConnection`: Chaîne de connexion à PostgreSQL
- `ASPNETCORE_ENVIRONMENT`: Development/Production

**Blazor:**
- `ApiBaseUrl`: URL de l'API (http://api:8080 en interne)

### Volumes

- `postgres_data`: Persiste les données de la base de données

## 🚀 Production

Pour le déploiement en production :

1. Modifier `ASPNETCORE_ENVIRONMENT` à `Production`
2. Utiliser des secrets pour les mots de passe
3. Configurer HTTPS avec des certificats
4. Ajuster les ressources (CPU, mémoire) selon les besoins

## 🔍 Troubleshooting

**Erreur "port already in use":**
```bash
# Arrêter les applications locales
dotnet stop (Ctrl+C dans les terminaux)

# Ou changer les ports dans docker-compose.yml
```

**La base de données n'est pas prête:**
```bash
# Le healthcheck attend que PostgreSQL soit prêt
# Vérifier les logs : docker-compose logs postgres
```

**Problèmes de build:**
```bash
# Nettoyer les images et rebuilder
docker-compose down --rmi all
docker-compose up --build
```
