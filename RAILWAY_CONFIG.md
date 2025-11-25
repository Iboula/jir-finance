# Configuration Railway pour JIR Finance

## Services déployés

### Service 1: API (jir-api)
**Dockerfile**: `Dockerfile` (racine: repository root)
**Variables d'environnement**:
- `ConnectionStrings__DefaultConnection`: Fournie automatiquement par Railway PostgreSQL
- `ASPNETCORE_ENVIRONMENT`: Production

**Configuration build**:
- Builder: DOCKERFILE
- Dockerfile Path: `Dockerfile` (défaut)
- Watch Paths: `src/JIR.WebAPI/**`, `src/JIR.Application/**`, `src/JIR.Domain/**`, `src/JIR.Infrastructure/**`

### Service 2: Blazor Frontend (jir-blazor) - À CONFIGURER MANUELLEMENT

⚠️ **Important**: Railway ne supporte qu'un seul Dockerfile par défaut dans `railway.json`. Pour déployer le Blazor, il faut créer un **second service** manuellement dans Railway Dashboard.

**Étapes de configuration dans Railway Dashboard**:

1. **Créer un nouveau service**:
   - Aller dans le projet Railway
   - Cliquer sur "+ New Service"
   - Sélectionner "GitHub Repo"
   - Choisir le repo `jir-finance`
   - Branch: `feature/docker-charts`

2. **Configurer le build**:
   - Root Directory: `src/JIR.BlazorApp`
   - Dockerfile Path: `Dockerfile.railway`
   
3. **Variables d'environnement**:
   - `API_URL`: URL de l'API Railway (ex: `https://jir-api.up.railway.app`)
   - `ASPNETCORE_ENVIRONMENT`: Production

4. **Configuration avancée**:
   - Watch Paths: `src/JIR.BlazorApp/**`
   - Port: 8080

## Dockerfiles

### Pour Docker Compose (développement local)
- **API**: `Dockerfile` (racine) - Utilise `src/JIR.WebAPI/` comme chemin
- **Blazor**: `src/JIR.BlazorApp/Dockerfile` - Utilise `src/JIR.BlazorApp/` comme chemin

### Pour Railway (production)
- **API**: `Dockerfile` (racine) - Fonctionne tel quel
- **Blazor**: `src/JIR.BlazorApp/Dockerfile.railway` - Adapté pour Railway (contexte: `src/JIR.BlazorApp`)

## Pourquoi deux Dockerfiles pour Blazor?

**Docker Compose** et **Railway** utilisent des contextes de build différents:

- **Docker Compose**: Build depuis la racine du repo
  - Context: `.` (racine)
  - Paths: `src/JIR.BlazorApp/...`

- **Railway**: Build depuis le sous-dossier
  - Root Directory: `src/JIR.BlazorApp`
  - Context: `src/JIR.BlazorApp` (dossier courant)
  - Paths: `.` (relatif au root directory)

## Base de données

**PostgreSQL** est fournie automatiquement par Railway:
- Service: `jir-postgres` 
- Connection string injectée automatiquement dans `ConnectionStrings__DefaultConnection`
