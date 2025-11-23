# Déploiement Azure Container Apps - JIR Finance

Ce guide explique comment déployer l'application JIR Finance sur Azure Container Apps.

## Prérequis

1. **Azure CLI** - Installez depuis: https://aka.ms/installazurecliwindows
2. **Compte Azure** avec un abonnement actif
3. **Container Registry** - rejirfinance.azurecr.io (déjà configuré)
4. **Images Docker** poussées vers ACR (✅ déjà fait)

## Architecture Déployée

```
┌─────────────────────────────────────────────────┐
│         Azure Container Apps Environment        │
│                                                  │
│  ┌──────────────┐      ┌──────────────┐        │
│  │   Blazor     │─────▶│     API      │        │
│  │   (Public)   │      │   (Public)   │        │
│  │  Port 8080   │      │  Port 8080   │        │
│  └──────────────┘      └──────┬───────┘        │
│                                │                 │
│                                ▼                 │
│                        ┌──────────────┐         │
│                        │  PostgreSQL  │         │
│                        │  (Internal)  │         │
│                        │  Port 5432   │         │
│                        └──────────────┘         │
└─────────────────────────────────────────────────┘
```

## Méthode 1: Script PowerShell (Recommandé)

### Étape 1: Installer Azure CLI

```powershell
# Télécharger et installer depuis: https://aka.ms/installazurecliwindows
# OU utiliser winget:
winget install -e --id Microsoft.AzureCLI
```

### Étape 2: Se connecter à Azure

```powershell
az login
```

### Étape 3: Exécuter le script de déploiement

```powershell
.\deploy-containerapp.ps1
```

Le script va:
- ✅ Créer le groupe de ressources `rg-jir-finance`
- ✅ Créer l'environnement Container Apps
- ✅ Déployer PostgreSQL (interne)
- ✅ Déployer l'API (publique)
- ✅ Déployer Blazor (publique)
- ✅ Afficher les URLs d'accès

**Durée estimée**: 5-10 minutes

## Méthode 2: Bicep Template

### Prérequis supplémentaires

Récupérer le mot de passe ACR:

```powershell
$acrPassword = az acr credential show --name rejirfinance --query "passwords[0].value" --output tsv
```

### Déployer

```powershell
# Créer le groupe de ressources
az group create --name rg-jir-finance --location canadacentral

# Déployer le template
az deployment group create `
  --resource-group rg-jir-finance `
  --template-file container-app.bicep `
  --parameters acrPassword=$acrPassword
```

## URLs après déploiement

Après un déploiement réussi, vous obtiendrez:

```
🌐 Application Blazor:  https://jir-blazor.xxx.canadacentral.azurecontainerapps.io
⚙️  API Backend:         https://jir-api.xxx.canadacentral.azurecontainerapps.io
🐘 PostgreSQL:          jir-postgres.internal.xxx.canadacentral.azurecontainerapps.io (interne)
```

## Commandes utiles

### Voir les logs

```powershell
# Logs de l'API
az containerapp logs show --name jir-api --resource-group rg-jir-finance --follow

# Logs de Blazor
az containerapp logs show --name jir-blazor --resource-group rg-jir-finance --follow

# Logs de PostgreSQL
az containerapp logs show --name jir-postgres --resource-group rg-jir-finance --follow
```

### Mettre à jour une image

```powershell
# Pousser nouvelle version vers ACR
docker push rejirfinance.azurecr.io/jir-api:latest
docker push rejirfinance.azurecr.io/jir-blazor:latest

# Container Apps va automatiquement détecter et redéployer
# OU forcer la mise à jour:
az containerapp update --name jir-api --resource-group rg-jir-finance
az containerapp update --name jir-blazor --resource-group rg-jir-finance
```

### Scaler l'application

```powershell
# Modifier le nombre de réplicas
az containerapp update `
  --name jir-api `
  --resource-group rg-jir-finance `
  --min-replicas 2 `
  --max-replicas 5
```

### Voir l'état des applications

```powershell
# Liste toutes les container apps
az containerapp list --resource-group rg-jir-finance --output table

# Détails d'une app
az containerapp show --name jir-blazor --resource-group rg-jir-finance
```

### Obtenir les URLs

```powershell
# URL de Blazor
az containerapp show `
  --name jir-blazor `
  --resource-group rg-jir-finance `
  --query "properties.configuration.ingress.fqdn" `
  --output tsv

# URL de l'API
az containerapp show `
  --name jir-api `
  --resource-group rg-jir-finance `
  --query "properties.configuration.ingress.fqdn" `
  --output tsv
```

## Coûts estimés

Container Apps utilise un modèle de tarification pay-per-use:

- **Environnement**: Gratuit
- **Compute**: ~$0.000024/vCPU-second + ~$0.000002/GiB-second
- **Estimation mensuelle** (3 apps, 24/7):
  - PostgreSQL: ~$15-20/mois (1 replica)
  - API: ~$15-30/mois (1-3 replicas)
  - Blazor: ~$15-30/mois (1-3 replicas)
  - **Total**: ~$45-80/mois

💡 **Astuce**: Utilisez `--min-replicas 0` pour les environnements de dev pour réduire les coûts.

## Supprimer les ressources

```powershell
# Supprimer tout le groupe de ressources
az group delete --name rg-jir-finance --yes --no-wait
```

## Troubleshooting

### Problème: Images non trouvées

```powershell
# Vérifier les images dans ACR
az acr repository list --name rejirfinance --output table

# Vérifier les tags
az acr repository show-tags --name rejirfinance --repository jir-api --output table
```

### Problème: L'API ne peut pas se connecter à PostgreSQL

Les Container Apps dans le même environnement peuvent communiquer via les noms de service internes:
- Format: `<app-name>.internal.<environment-fqdn>`
- Exemple: `jir-postgres.internal.xxx.canadacentral.azurecontainerapps.io`

### Problème: Blazor ne peut pas atteindre l'API

Vérifier que l'API est publique et que l'URL est correcte dans les variables d'environnement de Blazor.

## Monitoring

Azure Container Apps inclut:
- ✅ Logs automatiques dans Log Analytics
- ✅ Métriques CPU/Mémoire/Requêtes
- ✅ Application Insights (optionnel)
- ✅ Alertes et scaling automatique

Accéder au monitoring:
1. Portal Azure → Resource Group → rg-jir-finance
2. Cliquer sur une Container App
3. Menu "Monitoring" → Logs / Metrics

## Support

Pour plus d'informations:
- Documentation: https://learn.microsoft.com/azure/container-apps/
- Pricing: https://azure.microsoft.com/pricing/details/container-apps/
- Samples: https://github.com/Azure-Samples/container-apps-store-api-microservice
