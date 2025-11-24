# Guide de déploiement Railway - JIR Finance

## Projet Railway
URL: https://railway.com/project/990b025d-826c-4ab9-9876-a5a595392926

## Services à créer

### 1. PostgreSQL (DÉJÀ CRÉÉ ✓)
Base de données PostgreSQL déjà configurée.

### 2. Service API (jir-api)

#### Création:
1. Cliquez sur "+ New" dans le dashboard
2. Sélectionnez "GitHub Repo"
3. Choisissez: `Iboula/jir-finance`
4. Branche: `feature/docker-charts`
5. Nom du service: `jir-api`

#### Variables d'environnement:
```
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://0.0.0.0:$PORT
DATABASE_URL=${{Postgres.DATABASE_URL}}
```

**Note**: Le code convertit automatiquement `DATABASE_URL` au format Npgsql.

#### Settings:
- **Root Directory**: (laissez vide)
- **Dockerfile Path**: `src/JIR.WebAPI/Dockerfile`
- **Start Command**: (laissez vide, le Dockerfile gère ça)
- **Health Check Path**: `/health`

### 3. Service Blazor (jir-blazor)

#### Création:
1. Cliquez sur "+ New" dans le dashboard
2. Sélectionnez "GitHub Repo"
3. Choisissez: `Iboula/jir-finance`
4. Branche: `feature/docker-charts`
5. Nom du service: `jir-blazor`

#### Variables d'environnement:
```
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://0.0.0.0:$PORT
ApiBaseUrl=${{jir-api.url}}
```

#### Settings:
- **Root Directory**: (laissez vide)
- **Dockerfile Path**: `src/JIR.BlazorApp/Dockerfile`
- **Start Command**: (laissez vide)

## Ordre de déploiement

1. ✅ PostgreSQL (déjà fait)
2. 🔄 API (dépend de PostgreSQL)
3. 🔄 Blazor (dépend de l'API)

## Syntaxe Railway pour les variables

Railway utilise la syntaxe `${{SERVICE.VARIABLE}}` pour référencer les variables entre services:
- `${{Postgres.DATABASE_URL}}` - URL de connexion PostgreSQL
- `${{jir-api.url}}` - URL publique de l'API
- `$PORT` - Port assigné automatiquement par Railway

## Temps de déploiement estimé

- API: ~3-5 minutes (build Docker + déploiement)
- Blazor: ~3-5 minutes (build Docker + déploiement)

## Vérification

Une fois déployés, Railway vous donnera des URLs publiques:
- API: `https://jir-api-production-XXXX.up.railway.app`
- Blazor: `https://jir-blazor-production-XXXX.up.railway.app`

### Initialisation de la base de données:

**IMPORTANT**: La base de données n'est plus initialisée automatiquement au démarrage.

1. **Vérifier le statut de la base de données**:
   ```bash
   GET https://jir-api-production-XXXX.up.railway.app/api/seed/status
   ```

2. **Initialiser la base de données** (migrations + seed):
   ```bash
   POST https://jir-api-production-XXXX.up.railway.app/api/seed/initialize
   ```

3. **Vérifier la santé de l'API**:
   ```bash
   GET https://jir-api-production-XXXX.up.railway.app/health
   ```

### Test de l'application:
- API: Ajoutez `/swagger` à l'URL
- Blazor: Ouvrez directement l'URL

## Commandes CLI utiles

```bash
# Voir les logs
railway logs --service jir-api

# Voir le statut
railway status

# Redéployer
railway up --service jir-api

# Ouvrir le dashboard
railway open
```

## En cas de problème

1. Vérifiez les logs: Dashboard → Service → Logs
2. Vérifiez les variables: Dashboard → Service → Variables
3. Vérifiez le build: Dashboard → Service → Deployments
4. Contactez le support Railway si nécessaire

## Domaines personnalisés (optionnel)

Une fois l'application déployée, vous pouvez ajouter des domaines personnalisés:
1. Dashboard → Service → Settings → Domains
2. Ajoutez votre domaine
3. Configurez les DNS selon les instructions Railway
