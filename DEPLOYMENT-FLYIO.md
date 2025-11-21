# Déploiement sur Fly.io

## Applications déployées

### 1. API Backend (jir)
- **URL**: https://jir.fly.dev
- **Région**: Toronto (yyz)
- **Image**: mcr.microsoft.com/dotnet/aspnet:9.0
- **Configuration**: fly-api.toml
- **Dockerfile**: Dockerfile.api
- **État**: ✅ Déployé et actif

### 2. Application Blazor (jir-blazor)
- **URL**: https://jir-blazor.fly.dev
- **Région**: Toronto (yyz)
- **Image**: mcr.microsoft.com/dotnet/aspnet:9.0
- **Configuration**: fly-blazor.toml
- **Dockerfile**: Dockerfile.blazor
- **État**: 🔄 En cours de déploiement

## Base de données

### PostgreSQL
**Note**: Le cluster PostgreSQL initial a rencontré des erreurs. Options pour la production:

1. **Supabase** (Recommandé pour débuter)
   - Gratuit jusqu'à 500MB
   - Managed PostgreSQL
   - URL: https://supabase.com

2. **Neon** (Alternative)
   - Gratuit avec 512MB
   - Serverless PostgreSQL
   - URL: https://neon.tech

3. **Fly Postgres Managed**
   ```bash
   flyctl postgres create --name jir-postgres-managed --region yyz
   ```

## Commandes utiles

### Vérifier le statut des applications
```powershell
flyctl status -a jir
flyctl status -a jir-blazor
```

### Voir les logs
```powershell
flyctl logs -a jir
flyctl logs -a jir-blazor
```

### Redéployer une application
```powershell
flyctl deploy --config fly-api.toml --remote-only -a jir
flyctl deploy --config fly-blazor.toml --remote-only -a jir-blazor
```

### Ouvrir l'application dans le navigateur
```powershell
flyctl open -a jir
flyctl open -a jir-blazor
```

### Se connecter en SSH à une machine
```powershell
flyctl ssh console -a jir
flyctl ssh console -a jir-blazor
```

### Gérer les secrets (variables d'environnement)
```powershell
# Définir un secret
flyctl secrets set DATABASE_URL="postgres://..." -a jir

# Lister les secrets
flyctl secrets list -a jir

# Supprimer un secret
flyctl secrets unset DATABASE_URL -a jir
```

### Mettre à jour la connexion à la base de données
```powershell
# Une fois la base de données externe configurée
flyctl secrets set ConnectionStrings__DefaultConnection="Host=xxx;Port=5432;Database=xxx;Username=xxx;Password=xxx" -a jir
```

## Architecture de déploiement

```
┌─────────────────────────────────────────────┐
│         Utilisateur (Navigateur)            │
└─────────────────┬───────────────────────────┘
                  │
                  │ HTTPS
                  ▼
┌─────────────────────────────────────────────┐
│      jir-blazor.fly.dev (Blazor App)        │
│      - .NET 9.0 Blazor Server               │
│      - Port 8080                            │
│      - Auto-stop: Oui                       │
└─────────────────┬───────────────────────────┘
                  │
                  │ HTTPS/HTTP
                  ▼
┌─────────────────────────────────────────────┐
│         jir.fly.dev (API Backend)           │
│         - .NET 9.0 Web API                  │
│         - Port 8080                         │
│         - Auto-stop: Oui                    │
└─────────────────┬───────────────────────────┘
                  │
                  │ PostgreSQL
                  ▼
┌─────────────────────────────────────────────┐
│      Base de données (À configurer)         │
│      - Supabase / Neon / Fly Postgres       │
└─────────────────────────────────────────────┘
```

## Configuration de la base de données externe

### Option 1: Supabase (Recommandé)

1. Créer un compte sur https://supabase.com
2. Créer un nouveau projet
3. Récupérer la chaîne de connexion PostgreSQL
4. Mettre à jour la variable d'environnement:
   ```powershell
   flyctl secrets set ConnectionStrings__DefaultConnection="Host=db.xxx.supabase.co;Port=5432;Database=postgres;Username=postgres;Password=xxx" -a jir
   ```

### Option 2: Neon

1. Créer un compte sur https://neon.tech
2. Créer un nouveau projet
3. Récupérer la chaîne de connexion
4. Mettre à jour la variable d'environnement (même commande qu'au-dessus)

## Migration de la base de données

Une fois la base de données externe configurée:

```powershell
# 1. Restaurer le backup local vers la base externe
psql -h db.xxx.supabase.co -U postgres -d postgres -f backups/jir_backup.sql

# 2. Ou utiliser un client PostgreSQL (pgAdmin, DBeaver, etc.)
```

## Coûts estimés (Free Tier)

- **Fly.io**: Gratuit pour 3 machines partagées (jusqu'à 256MB RAM chacune)
- **Supabase**: Gratuit jusqu'à 500MB de base de données
- **Neon**: Gratuit jusqu'à 512MB de base de données

**Total estimé**: $0/mois pour commencer

## Notes importantes

1. Les machines s'arrêtent automatiquement après inactivité (auto_stop_machines)
2. Elles redémarrent automatiquement lors de la première requête
3. Le premier chargement après une période d'inactivité peut prendre quelques secondes
4. Pour une production avec haute disponibilité, configurer `min_machines_running = 1`

## Prochaines étapes

1. ✅ Déployer l'API
2. 🔄 Déployer Blazor
3. ⏳ Configurer une base de données externe (Supabase/Neon)
4. ⏳ Restaurer les données
5. ⏳ Tester l'application en production
6. ⏳ Configurer un domaine personnalisé (optionnel)
