# 🚀 Équipe d'Agents MCP Spécialisés - Système Financier JIR

## 📋 Vue d'ensemble

Cette suite d'agents MCP est conçue pour accélérer le développement d'un système de digitalisation financière complet basé sur :

- **Frontend** : Blazor WebAssembly + Radzen UI
- **Backend** : .NET 9 Web API + Clean Architecture + CQRS
- **Base de données** : PostgreSQL + EF Core 9
- **Infrastructure** : Docker Compose + CI/CD

## 👥 Équipe d'Agents

| Agent | Rôle | Outils MCP | Priorité |
|-------|------|-----------|----------|
| **Architecte.NET** | Architecture backend, CQRS, API | Codebase, Filesystem, SQL, WebSearch | ⭐⭐⭐ |
| **DB/PostgreSQL Engineer** | Schéma DB, migrations, optimisations | SQL, Codebase | ⭐⭐⭐ |
| **Blazor Frontend Engineer** | Pages, composants, UI, auth | Codebase, Filesystem, WebSearch | ⭐⭐⭐ |
| **DevOps/Docker Engineer** | Containerisation, CI/CD, déploiement | Codebase, Filesystem, GitHub | ⭐⭐ |
| **PDF & Document Automation** | Génération PDF, signatures électroniques | PDF, Codebase | ⭐⭐ |
| **Documentation & UML** | Docs techniques, diagrammes, guides | Document, WebSearch | ⭐ |

## 🎯 Workflow de Collaboration

### Phase 1 : Fondations (Agents 1, 2)
```
1. DB/PostgreSQL Engineer → Crée le schéma DB
2. Architecte.NET → Génère l'architecture Clean + CQRS
3. Les deux agents synchronisent les modèles de domaine
```

### Phase 2 : Développement (Agents 3, 4)
```
4. Architecte.NET → Implémente les handlers CQRS + API endpoints
5. Blazor Frontend Engineer → Crée les pages et composants
6. DevOps Engineer → Prépare Docker Compose
```

### Phase 3 : Fonctionnalités Avancées (Agent 5)
```
7. PDF & Document Automation → Génère templates + workflows
8. Tous les agents → Tests et intégration
```

### Phase 4 : Finalisation (Agent 6)
```
9. Documentation & UML → Documente tout le projet
10. DevOps Engineer → Configure CI/CD final
```

## 📂 Structure Générée

```
JIR/
├── src/
│   ├── JIR.Domain/              # Généré par Architecte.NET
│   ├── JIR.Application/         # Généré par Architecte.NET
│   ├── JIR.Infrastructure/      # Généré par Architecte.NET + DB Engineer
│   ├── JIR.WebAPI/              # Généré par Architecte.NET
│   └── JIR.BlazorApp/           # Généré par Blazor Engineer
├── tests/                       # Généré par Architecte.NET
├── docker/                      # Généré par DevOps Engineer
├── docs/                        # Généré par Documentation Agent
└── scripts/                     # Généré par tous les agents
```

## 🚀 Utilisation

### Étape 1 : Charger les Agents
Importez tous les fichiers JSON d'agents dans votre outil compatible MCP (Cursor, Windsurf, ChatGPT Workspace).

### Étape 2 : Initialiser le Projet
```bash
# L'agent DevOps créera ce script
./scripts/init-project.ps1
```

### Étape 3 : Lancer les Agents dans l'Ordre
1. **DB/PostgreSQL Engineer** : `@db-engineer Crée le schéma complet basé sur cahier-charges.md`
2. **Architecte.NET** : `@architecte Génère l'architecture Clean + CQRS pour tous les modules`
3. **Blazor Frontend Engineer** : `@blazor-engineer Crée toutes les pages CRUD + dashboards`
4. **DevOps Engineer** : `@devops Configure Docker Compose et CI/CD`
5. **PDF Agent** : `@pdf-agent Génère tous les templates PDF de rapports`
6. **Documentation Agent** : `@doc-agent Documente l'intégralité du projet`

## 🔥 Fonctionnalités par Module

### Module Sections
- CRUD sections
- API endpoints
- Pages Blazor
- Tests

### Module Cotisations
- Gestion des cotisants
- Calculs automatiques
- Historique
- Rapports Excel/PDF

### Module Dépenses
- Workflow de signatures électroniques
- Upload de justificatifs
- Validation multi-niveaux
- États financiers

### Module Recettes
- Enregistrement recettes
- Catégorisation
- Rapports

### Module Partenaires
- Annuaire partenaires
- Contrats
- Suivi collaborations

### Module Magasins
- Gestion stocks
- Bons d'entrée/sortie
- Inventaire

### Module Rapports
- Solde caisse automatique
- Rapports financiers
- Export PDF
- Dashboards interactifs

## 🛠️ Commandes Utiles

### Générer un Module Complet
```
@architecte + @blazor-engineer Créez le module [NOM_MODULE] avec :
- Entités Domain
- Commandes/Queries CQRS
- API Controllers
- Pages Blazor CRUD
- Tests
```

### Ajouter une Fonctionnalité
```
@architecte Ajoute la fonctionnalité [FONCTIONNALITÉ] au module [MODULE]
```

### Optimiser la Base de Données
```
@db-engineer Analyse et optimise les requêtes du module [MODULE]
```

### Générer Documentation
```
@doc-agent Génère la documentation complète du module [MODULE]
```

## 📦 Dépendances Requises

Chaque agent vérifiera et installera automatiquement :

- .NET 9 SDK
- PostgreSQL 16
- Docker Desktop
- Node.js (pour Blazor tooling)
- Outils de génération PDF

## 🎓 Best Practices

1. **Toujours commencer par la DB** : Le schéma PostgreSQL est la fondation
2. **Architecture first** : Laissez l'Architecte.NET créer la structure avant le code métier
3. **Tests continus** : Chaque agent génère ses propres tests
4. **Docker dès le début** : DevOps Engineer configure l'environnement tôt
5. **Documentation incrémentale** : Documentation Agent documente au fur et à mesure

## 🔒 Sécurité

Les agents intègrent automatiquement :
- ✅ JWT Authentication
- ✅ Role-based Authorization
- ✅ Input Validation
- ✅ SQL Injection Protection
- ✅ XSS Protection
- ✅ HTTPS obligatoire
- ✅ Signature électronique sécurisée

## 📞 Support

Pour toute question sur l'utilisation des agents, consultez :
- `agents/[NOM_AGENT]/README.md` pour chaque agent
- `docs/architecture.md` pour la vision globale
- `docs/workflows.md` pour les processus métier

---

**Développé pour le projet JIR - Système de Gestion Financière**
