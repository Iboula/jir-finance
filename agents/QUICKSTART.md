# 🚀 Guide de Démarrage Rapide - Agents MCP JIR

## 📌 Introduction

Ce guide vous explique comment utiliser la suite d'agents MCP pour développer rapidement votre système de gestion financière JIR.

---

## 🎯 Étape 1 : Importer les Agents

### Pour ChatGPT Workspace / Agents
1. Ouvrez ChatGPT
2. Allez dans **Agents** ou **Workspace**
3. Créez un nouvel agent
4. Copiez le contenu de chaque fichier JSON (`1-architecte-dotnet.json`, etc.)
5. Collez dans la configuration de l'agent
6. Répétez pour les 6 agents

### Pour Cursor / Windsurf
1. Placez tous les fichiers JSON dans le dossier `.cursor/agents/` ou `.windsurf/agents/`
2. Redémarrez l'IDE
3. Les agents seront automatiquement détectés

### Pour VS Code Copilot (avec support MCP)
1. Installez l'extension compatible MCP
2. Importez les agents via le panneau d'agents
3. Activez les agents nécessaires

---

## ⚡ Étape 2 : Workflow d'Utilisation

### Phase 1 : Initialisation (Jour 1)

#### 1. Démarrer avec l'Agent DB/PostgreSQL Engineer

```
@db-engineer Crée le schéma PostgreSQL complet basé sur le cahier des charges suivant :
- Sections (code, nom, description, budget, responsable)
- Cotisations (membre, montant, période, date paiement, reçu)
- Dépenses (libellé, montant, workflow validation, signatures)
- Recettes (source, montant, catégorie)
- Partenaires (nom, type, contact)
- Magasins et articles (stock, mouvements)
- Users (auth, roles)

Génère aussi :
- Les scripts de création SQL
- Les scripts de seed avec données de test
- Les index et contraintes
- Les views pour les rapports
```

**Résultat attendu** : 
- ✅ Scripts SQL prêts dans `scripts/sql/`
- ✅ Schéma documenté

---

#### 2. Créer l'Architecture Backend avec Architecte.NET

```
@architecte Initialise le projet complet avec :
- Solution JIR avec Clean Architecture
- Projets : Domain, Application, Infrastructure, WebAPI, Tests
- Structure CQRS complète
- Configuration EF Core + PostgreSQL
- Authentification JWT
- Swagger

Crée ensuite tous les modules :
1. Sections (CRUD complet)
2. Cotisations
3. Dépenses (avec workflow de validation)
4. Recettes
5. Partenaires
6. Magasins + Articles + Mouvements Stock
7. Rapports (solde caisse, états financiers)
```

**Résultat attendu** :
- ✅ Solution .NET complète
- ✅ Tous les modules avec Commands/Queries/Handlers
- ✅ Migrations EF Core générées
- ✅ API fonctionnelle avec Swagger

---

### Phase 2 : Frontend (Jour 2)

#### 3. Générer l'Application Blazor

```
@blazor-engineer Crée l'application Blazor WebAssembly complète avec Radzen :

1. Initialise le projet avec Radzen Components
2. Configure l'authentification JWT côté client
3. Crée tous les services API pour communiquer avec le backend
4. Génère toutes les pages :
   - Dashboard (4 cards stats + 2 graphiques)
   - CRUD Sections
   - CRUD Cotisations avec filtres
   - Formulaire Dépenses en 3 étapes (info, upload, signature)
   - Page validation dépenses
   - CRUD Recettes
   - CRUD Partenaires
   - Gestion Stock (articles, mouvements entrée/sortie)
   - Rapports financiers avec exports PDF/Excel
5. Crée les composants :
   - SignaturePad (signature électronique)
   - FileUploader
   - CurrencyInput
```

**Résultat attendu** :
- ✅ Application Blazor fonctionnelle
- ✅ Toutes les pages CRUD
- ✅ Dashboard interactif
- ✅ Authentification intégrée

---

### Phase 3 : DevOps (Jour 2-3)

#### 4. Containeriser avec DevOps Engineer

```
@devops Génère l'infrastructure complète :

1. Docker Compose avec :
   - PostgreSQL + PgAdmin
   - API .NET
   - Blazor WebAssembly
   - NGINX reverse proxy
2. Dockerfiles optimisés (multi-stage builds)
3. Configuration NGINX
4. Scripts PowerShell :
   - init-project.ps1 (initialisation)
   - deploy.ps1 (déploiement)
   - backup-db.ps1 (sauvegarde)
5. Pipeline CI/CD GitHub Actions
6. Fichier .env.example
7. Health checks pour tous les services
```

**Résultat attendu** :
- ✅ Docker Compose fonctionnel
- ✅ Application démarrable avec une commande
- ✅ CI/CD configuré

---

### Phase 4 : Documents (Jour 3)

#### 5. Générer les PDF avec PDF Agent

```
@pdf-agent Implémente la génération automatique de tous les documents :

1. Bon de dépense avec signatures électroniques et QR Code
2. Reçu de cotisation avec montant en lettres
3. Rapport financier complet (multi-pages)
4. Bon de sortie magasin
5. État de stock avec alertes

Configure aussi :
- Service de génération QuestPDF
- Workflow de signatures électroniques
- Stockage des PDF
- Endpoints API pour téléchargement
```

**Résultat attendu** :
- ✅ Tous les templates PDF fonctionnels
- ✅ Génération accessible depuis Blazor
- ✅ QR Codes pour vérification

---

### Phase 5 : Documentation (Jour 4)

#### 6. Documenter avec Documentation Agent

```
@doc-agent Génère la documentation complète :

1. README principal du projet
2. Diagrammes UML :
   - Classes (domaine)
   - Séquence (login, dépense)
   - Composants (architecture)
   - Déploiement (Docker)
   - ER (base de données)
3. Guide utilisateur pour tous les modules
4. Guide d'installation (Docker + manuel)
5. Documentation API (complément Swagger)
6. ADR (décisions architecturales)
7. Guide de contribution
```

**Résultat attendu** :
- ✅ Documentation complète dans `docs/`
- ✅ Diagrammes UML à jour
- ✅ Guides utilisateur et admin

---

## 🔥 Commandes Rapides

### Initialiser le projet
```powershell
# Cloner/créer le dossier
cd c:\Users\iboul\OneDrive\Documents\PROJETS\MOPRO\JIR

# Exécuter le script d'initialisation (généré par DevOps)
.\scripts\init-project.ps1
```

### Démarrer l'application
```powershell
# Avec Docker
docker-compose up -d

# URLs
# Frontend: http://localhost:5001
# API: http://localhost:5000
# Swagger: http://localhost:5000/swagger
# PgAdmin: http://localhost:5050
```

### Générer un nouveau module
```
@architecte + @blazor-engineer Créez le module [NOM_MODULE] avec :
- Entités Domain
- Commands/Queries CQRS
- API Controllers
- Pages Blazor CRUD
- Tests unitaires
```

---

## 🎨 Prompts Utiles par Scénario

### Scénario 1 : Ajouter une fonctionnalité
```
@architecte Ajoute la fonctionnalité "Export Excel des cotisations" :
1. Crée une nouvelle Query GetCotisationsForExportQuery
2. Crée le Handler correspondant
3. Ajoute l'endpoint /api/cotisations/export

@blazor-engineer Ajoute un bouton "Export Excel" sur la page CotisationsList.razor
```

### Scénario 2 : Modifier le schéma DB
```
@db-engineer Ajoute une colonne "taux_cotisation" (decimal) à la table sections

@architecte Mets à jour l'entité Section et génère la migration EF Core

@blazor-engineer Ajoute le champ "Taux Cotisation" dans le formulaire SectionForm.razor
```

### Scénario 3 : Créer un nouveau rapport
```
@db-engineer Crée une view "v_cotisations_par_mois" qui agrège les cotisations par mois et section

@architecte Crée une Query GetCotisationsParMoisQuery pour récupérer les données de la view

@blazor-engineer Crée la page RapportCotisationsMensuel.razor avec un tableau et un graphique

@pdf-agent Génère un template PDF pour ce rapport
```

### Scénario 4 : Optimiser les performances
```
@db-engineer Analyse les requêtes lentes et propose des index

@architecte Implémente le caching avec Redis pour les rapports fréquents

@blazor-engineer Implémente la pagination virtualisée sur les grandes listes
```

---

## 🧠 Coordination des Agents

### Ordre recommandé pour chaque module :

1. **DB Engineer** : Crée/modifie le schéma
2. **Architecte.NET** : Génère les entités, CQRS, API
3. **Blazor Engineer** : Crée les pages et services
4. **PDF Agent** : Génère les templates PDF (si nécessaire)
5. **DevOps Engineer** : Ajuste Docker si nouveaux services
6. **Documentation Agent** : Documente le module

### Communication entre agents :

```
DB Engineer ──[Schéma SQL]──> Architecte.NET
                                    │
                                    ├─[DTOs, Endpoints]──> Blazor Engineer
                                    │
                                    └─[Entités]──> PDF Agent

DevOps Engineer ◄──[Config]── Tous les agents

Documentation Agent ◄──[Infos]── Tous les agents
```

---

## ✅ Checklist de Validation

Après avoir utilisé tous les agents, vérifiez :

### Backend (Architecte.NET + DB Engineer)
- [ ] La solution .NET compile sans erreur
- [ ] Toutes les migrations EF Core sont appliquées
- [ ] Les tests unitaires passent (`dotnet test`)
- [ ] Swagger affiche tous les endpoints
- [ ] L'authentification JWT fonctionne
- [ ] Les endpoints retournent des données valides

### Frontend (Blazor Engineer)
- [ ] L'application Blazor se lance sans erreur
- [ ] Le login fonctionne
- [ ] Toutes les pages CRUD sont fonctionnelles
- [ ] Les formulaires valident correctement
- [ ] Les appels API fonctionnent
- [ ] Le dashboard affiche les bonnes données

### DevOps (DevOps Engineer)
- [ ] `docker-compose up` démarre tous les services
- [ ] PostgreSQL est accessible
- [ ] L'API répond sur http://localhost:5000
- [ ] Blazor est accessible sur http://localhost:5001
- [ ] PgAdmin se connecte à PostgreSQL
- [ ] Les logs sont visibles

### Documents (PDF Agent)
- [ ] Les PDF se génèrent sans erreur
- [ ] Les QR Codes sont scannables
- [ ] Les signatures s'affichent
- [ ] Les téléchargements fonctionnent

### Documentation (Documentation Agent)
- [ ] README.md est complet
- [ ] Les diagrammes UML sont corrects
- [ ] Les guides sont clairs
- [ ] Tous les liens fonctionnent

---

## 🔧 Dépannage

### Problème : L'agent ne répond pas correctement
**Solution** : Reformulez votre prompt en étant plus spécifique et en incluant le contexte.

### Problème : Conflit entre agents
**Solution** : Suivez l'ordre recommandé (DB -> Backend -> Frontend -> DevOps -> Docs).

### Problème : Code généré avec erreurs
**Solution** : Demandez à l'agent de corriger en lui montrant l'erreur exacte.

### Problème : Documentation obsolète
**Solution** : Après chaque modification majeure, demandez à @doc-agent de mettre à jour.

---

## 📞 Support

Pour toute question :
1. Consultez les fichiers JSON des agents (documentation détaillée)
2. Consultez `docs/developer-guide/faq.md` (généré par @doc-agent)
3. Vérifiez les logs Docker : `docker-compose logs -f`

---

## 🎓 Bonnes Pratiques

1. **Toujours commencer par la base de données** (DB Engineer)
2. **Tester au fur et à mesure** (ne pas tout générer d'un coup)
3. **Commit régulièrement** (après chaque agent majeur)
4. **Documenter au fil de l'eau** (pas seulement à la fin)
5. **Valider avec de vraies données** (seed data réalistes)
6. **Utiliser Docker dès le début** (évite les "ça marche sur ma machine")

---

## 🚀 Pour Aller Plus Loin

Une fois le système de base fonctionnel :

```
@architecte Ajoute les notifications en temps réel avec SignalR

@blazor-engineer Implémente les notifications push dans le dashboard

@devops Configure le monitoring avec Prometheus et Grafana

@pdf-agent Ajoute des templates pour les contrats partenaires

@doc-agent Crée un guide d'administration avancé
```

---

**🎉 Félicitations ! Vous êtes prêt à accélérer votre développement d'un facteur x10 avec les agents MCP !**
