# 📊 Cahier des Charges - Système de Gestion Financière JIR

## 📌 Contexte

Organisation nécessitant la digitalisation complète de son système financier pour :
- Améliorer la traçabilité
- Automatiser les workflows
- Générer des rapports en temps réel
- Sécuriser les validations (signatures électroniques)
- Centraliser la gestion documentaire

---

## 🎯 Objectifs

1. **Digitaliser** tous les processus financiers (cotisations, dépenses, recettes)
2. **Automatiser** les workflows de validation avec signatures électroniques
3. **Générer** automatiquement les documents PDF (bons, reçus, rapports)
4. **Fournir** des dashboards interactifs pour le suivi en temps réel
5. **Gérer** les stocks des magasins
6. **Centraliser** les données dans une base PostgreSQL
7. **Déployer** facilement avec Docker

---

## 📦 Modules Fonctionnels

### 1. Module SECTIONS

**Description** : Gestion des différentes sections de l'organisation.

**Fonctionnalités** :
- CRUD sections
- Affectation de responsables
- Gestion du budget annuel
- Historique des modifications

**Données** :
- Code (unique)
- Nom
- Description
- Responsable (référence User)
- Budget annuel
- Statut (Actif/Inactif)

---

### 2. Module COTISATIONS

**Description** : Gestion des cotisations des membres.

**Fonctionnalités** :
- Enregistrement des cotisations
- Génération automatique de reçus PDF
- Historique par membre
- Recherche avancée (période, section, membre)
- Calcul des totaux par période
- Export Excel
- Statistiques (taux de paiement, retards)

**Données** :
- Section
- Nom et Matricule du membre
- Montant
- Période (YYYY-MM)
- Date de paiement
- Mode de paiement (Espèces, Virement, Chèque, Mobile Money)
- Numéro de reçu (auto-généré)
- Statut (Payé, En attente, Annulé)
- Notes

**Documents générés** :
- Reçu de cotisation (PDF) avec QR Code

---

### 3. Module DÉPENSES

**Description** : Gestion complète des dépenses avec workflow de validation.

**Fonctionnalités** :
- Création de demandes de dépense
- Workflow de validation multi-niveaux :
  1. Brouillon (création)
  2. En attente validation (niveau 1)
  3. En attente validation (niveau 2)
  4. Validé
  5. Rejeté
  6. Payé
- Signatures électroniques à chaque niveau
- Upload de justificatifs (PDF, images)
- Notifications automatiques aux validateurs
- Génération de bons de dépense PDF
- Traçabilité complète
- Recherche et filtres avancés

**Données** :
- Numéro de bon (auto-généré)
- Section
- Libellé
- Montant
- Date de dépense
- Bénéficiaire
- Catégorie (Fonctionnement, Investissement, Personnel, etc.)
- Mode de paiement
- Statut workflow
- Demandeur
- Validateurs (niveau 1 et 2)
- Dates de validation
- Signatures (images base64)
- Chemin du justificatif
- Motif (si rejeté)
- Notes

**Documents générés** :
- Bon de dépense (PDF) avec signatures et QR Code

---

### 4. Module RECETTES

**Description** : Enregistrement de toutes les recettes (hors cotisations).

**Fonctionnalités** :
- CRUD recettes
- Catégorisation
- Génération de reçus PDF
- Recherche et filtres
- Statistiques par source/catégorie
- Export Excel

**Données** :
- Numéro de reçu (auto-généré)
- Section
- Libellé
- Montant
- Date de recette
- Source (Subvention, Don, Vente, Autre)
- Catégorie
- Mode d'encaissement
- Notes

**Documents générés** :
- Reçu de recette (PDF)

---

### 5. Module PARTENAIRES

**Description** : Annuaire des partenaires (fournisseurs, clients, sponsors).

**Fonctionnalités** :
- CRUD partenaires
- Gestion des contrats
- Historique des transactions
- Recherche avancée
- Export Excel

**Données** :
- Code (unique)
- Nom
- Type (Fournisseur, Client, Sponsor, Autre)
- Adresse
- Téléphone
- Email
- Nom du contact principal
- Notes

---

### 6. Module MAGASINS

**Description** : Gestion des stocks dans les magasins.

**Fonctionnalités** :
- CRUD magasins
- Gestion des articles (code, désignation, unité)
- Mouvements de stock :
  - Entrées (achats, dons)
  - Sorties (utilisation, vente)
- Suivi du stock en temps réel
- Alertes stock faible (seuil paramétrable)
- Génération de bons d'entrée/sortie PDF
- État de stock (inventaire)
- Historique des mouvements

**Données Magasins** :
- Code
- Nom
- Localisation
- Responsable

**Données Articles** :
- Code article (unique)
- Désignation
- Unité (Kg, L, Unité, etc.)
- Quantité en stock
- Seuil d'alerte
- Prix unitaire moyen

**Données Mouvements** :
- Type (Entrée/Sortie)
- Article
- Quantité
- Date
- Numéro de document
- Motif
- Créé par

**Documents générés** :
- Bon d'entrée (PDF)
- Bon de sortie (PDF)
- État de stock (PDF)

---

### 7. Module RAPPORTS

**Description** : Génération de rapports financiers et analytiques.

**Fonctionnalités** :

#### 7.1 Solde Caisse
- Calcul automatique par section :
  - Total cotisations
  - Total recettes
  - Total dépenses
  - Solde = (Cotisations + Recettes) - Dépenses
- Solde global (toutes sections)
- Évolution sur période personnalisée
- Export PDF et Excel

#### 7.2 Rapport Financier Périodique
- Paramètres : Date début, Date fin, Section(s)
- Contenu :
  - Résumé exécutif
  - Tableau des cotisations
  - Tableau des recettes
  - Tableau des dépenses (par catégorie)
  - Graphiques :
    - Évolution mensuelle (ligne)
    - Répartition par section (camembert)
    - Dépenses par catégorie (barres)
  - Analyse comparative
  - Annexes (détails)
- Export PDF multi-pages

#### 7.3 Tableau de Bord (Dashboard)
- Indicateurs clés (KPI cards) :
  - Solde caisse actuel
  - Cotisations du mois
  - Dépenses du mois
  - Recettes du mois
- Graphiques :
  - Évolution sur 12 mois
  - Répartition par section
- Listes :
  - Dépenses en attente de validation (top 5)
  - Alertes stock faible (top 5)
  - Dernières opérations (top 10)

#### 7.4 Autres Rapports
- Suivi des cotisations par membre
- Analyse des dépenses par catégorie
- Rapport d'activité par section
- État des créances (si module crédit activé)

---

## 🔐 Module AUTHENTIFICATION & AUTORISATION

**Fonctionnalités** :
- Login / Logout
- Gestion des utilisateurs
- Rôles :
  - **Admin** : Accès total
  - **Manager** : Gestion de sa section + validation niveau 1
  - **User** : Création dépenses, consultation
  - **Viewer** : Lecture seule
- JWT Token (refresh automatique)
- Gestion des sessions
- Historique des connexions

**Données Users** :
- Username (unique)
- Email
- Password (hasher avec bcrypt)
- Prénom, Nom
- Rôle
- Section (optionnelle)
- Statut (Actif/Inactif)
- Dernière connexion

---

## 🖼️ Interface Utilisateur (Blazor + Radzen)

### Exigences UI/UX :
- Design moderne et responsive (mobile, tablet, desktop)
- Navigation intuitive (sidebar + top bar)
- Thème : Professionnel (couleurs bleu/gris)
- Composants Radzen :
  - RadzenDataGrid (tableaux)
  - RadzenDialog (modales)
  - RadzenNotification (toasts)
  - RadzenChart (graphiques)
  - RadzenButton, Forms, etc.
- Loading indicators partout
- Gestion des erreurs user-friendly
- Confirmation avant suppressions
- Formulaires avec validation temps réel

### Pages Principales :
1. **Login** (public)
2. **Dashboard** (accueil)
3. **Sections** (liste + form)
4. **Cotisations** (liste + form + détails)
5. **Dépenses** (liste + form wizard + validation)
6. **Recettes** (liste + form)
7. **Partenaires** (liste + form)
8. **Magasins** (liste + form)
9. **Stock** (articles + mouvements)
10. **Rapports** (multiples vues)
11. **Utilisateurs** (admin only)
12. **Profil** (utilisateur connecté)

---

## 🏗️ Architecture Technique

### Backend : .NET 9 Web API
- **Pattern** : Clean Architecture (Onion)
- **Couches** :
  1. **Domain** : Entités, Value Objects, Interfaces
  2. **Application** : CQRS (Commands/Queries), Handlers, DTOs, Validators
  3. **Infrastructure** : EF Core, Repositories, Services externes
  4. **WebAPI** : Controllers, Middleware
- **CQRS** : MediatR
- **Validation** : FluentValidation
- **Mapping** : AutoMapper
- **ORM** : Entity Framework Core 9
- **Authentification** : JWT Bearer
- **Documentation API** : Swagger/OpenAPI
- **Logging** : Serilog (console + PostgreSQL)
- **Tests** : xUnit + Moq + FluentAssertions

### Frontend : Blazor WebAssembly
- **UI Library** : Radzen Blazor Components
- **State Management** : Custom State Container ou Fluxor
- **HTTP** : HttpClient avec authentification JWT
- **LocalStorage** : Blazored.LocalStorage (pour token)
- **Validation** : DataAnnotations
- **Charts** : Radzen Charts

### Base de Données : PostgreSQL 16
- **Naming** : snake_case
- **Contraintes** : FK, CHECK, UNIQUE
- **Index** : Sur toutes les FK + colonnes de recherche
- **Triggers** : updated_at automatique, update stock
- **Views** : Rapports agrégés
- **Seed** : Données de test

### Infrastructure : Docker
- **Services** :
  - PostgreSQL (port 5432)
  - PgAdmin (port 5050)
  - API .NET (port 5000)
  - Blazor (port 5001)
  - NGINX (ports 80/443)
- **Volumes** : Persistance PostgreSQL
- **Networks** : Réseau bridge dédié
- **Health Checks** : Tous les services

### CI/CD : GitHub Actions
- Build .NET
- Run tests
- Build Docker images
- Deploy (staging/production)

---

## 📄 Génération de Documents PDF

### Bibliothèque : QuestPDF

### Documents à générer :
1. **Reçu de Cotisation**
   - Header avec logo
   - Numéro de reçu
   - Informations cotisant
   - Montant (chiffres + lettres)
   - QR Code
   - Signature responsable

2. **Bon de Dépense**
   - Numéro de bon
   - Détails dépense
   - Workflow (étapes + statut)
   - Signatures électroniques (images)
   - QR Code de vérification
   - Watermark si brouillon

3. **Bon de Sortie Magasin**
   - Liste articles
   - Quantités
   - Bénéficiaire
   - Signatures

4. **Rapport Financier**
   - Multi-pages
   - Tables + graphiques (images)
   - Mise en page professionnelle

### Fonctionnalités PDF :
- QR Code pour vérification en ligne
- Signatures électroniques
- Watermark conditionnel
- Compression optimale
- Métadonnées (auteur, date)
- Polices embarquées

---

## 🔄 Workflows Métier

### Workflow Dépense :
1. **Demandeur** crée dépense (statut : Brouillon)
2. **Demandeur** signe et soumet (statut : En attente validation)
3. **Notification** envoyée au Validateur 1
4. **Validateur 1** approuve et signe (statut : En attente validation niveau 2)
5. **Notification** envoyée au Validateur 2
6. **Validateur 2** approuve et signe (statut : Validé)
7. **Bon de dépense** généré automatiquement (PDF)
8. **Paiement** effectué (statut : Payé)

**Cas de rejet** : À tout moment, un validateur peut rejeter (avec motif).

### Workflow Stock :
1. **Magasinier** crée mouvement (entrée/sortie)
2. **Stock** mis à jour automatiquement (trigger DB)
3. **Alerte** si stock < seuil
4. **Bon** généré (PDF)

---

## 🔒 Sécurité

### Exigences :
- Authentification JWT obligatoire
- Refresh token automatique
- Rôles et permissions (RBAC)
- Validation des inputs (client + serveur)
- Protection CSRF
- HTTPS obligatoire en production
- Hashing mots de passe (bcrypt)
- Protection contre SQL Injection (EF Core)
- XSS Protection
- Rate limiting sur API
- Logging des actions sensibles
- Backup automatique DB

---

## 📊 Performances

### Objectifs :
- Temps de réponse API < 200ms (95e percentile)
- Chargement pages Blazor < 2s
- Support 100 utilisateurs concurrents
- Base de données scalable (indexation)

### Optimisations :
- Pagination sur toutes les listes
- Lazy loading Blazor
- Caching des données statiques
- Index PostgreSQL optimaux
- Requêtes EF Core optimisées (LINQ)
- Compression des réponses API

---

## 🧪 Tests

### Types de tests :
- **Tests unitaires** : Handlers CQRS (couverture > 80%)
- **Tests d'intégration** : API endpoints
- **Tests E2E** : Workflows complets (optionnel)
- **Tests de performance** : Load testing (optionnel)

---

## 📚 Documentation

### Livrables :
- README principal
- Diagrammes UML (classes, séquence, composants)
- Guide d'installation
- Guide utilisateur (par module)
- Guide administrateur
- Documentation API (Swagger + complément)
- ADR (décisions architecturales)

---

## 🚀 Déploiement

### Environnements :
- **Development** : Local (Docker Compose)
- **Staging** : Serveur de test
- **Production** : Serveur dédié ou cloud

### Procédure :
1. Build des images Docker
2. Push vers registry
3. Pull sur serveur
4. Exécution migrations DB
5. Démarrage containers
6. Health checks
7. Monitoring

---

## 📅 Livrables Attendus

1. ✅ **Code source complet** (GitHub)
2. ✅ **Base de données** (scripts SQL + migrations)
3. ✅ **Application Blazor** déployable
4. ✅ **API .NET** avec Swagger
5. ✅ **Docker Compose** fonctionnel
6. ✅ **Documents PDF** (templates)
7. ✅ **Documentation** complète
8. ✅ **Tests** (unitaires + intégration)
9. ✅ **CI/CD** (GitHub Actions)

---

## 📞 Stakeholders

- **Utilisateurs finaux** : Comptables, Responsables de section, Magasiniers
- **Administrateurs** : IT Admin
- **Décideurs** : Direction, Trésorier général

---

## ⚙️ Technologies Imposées

| Composant | Technologie |
|-----------|-------------|
| Backend | .NET 9 Web API |
| Frontend | Blazor WebAssembly |
| UI Library | Radzen Components |
| Database | PostgreSQL 16 |
| ORM | Entity Framework Core 9 |
| Pattern | Clean Architecture + CQRS |
| Auth | JWT Bearer |
| PDF | QuestPDF |
| Container | Docker + Docker Compose |
| Reverse Proxy | NGINX |
| CI/CD | GitHub Actions |

---

## 🎯 Critères de Succès

1. ✅ Tous les modules sont fonctionnels
2. ✅ Les workflows de validation fonctionnent
3. ✅ Les PDF sont générés correctement
4. ✅ L'authentification est sécurisée
5. ✅ L'application est responsive
6. ✅ Les performances sont acceptables
7. ✅ La documentation est complète
8. ✅ Le déploiement Docker fonctionne
9. ✅ Les tests passent (> 80% couverture)
10. ✅ Le code respecte les standards Clean Architecture

---

**Version** : 1.0  
**Date** : 2025-11-17  
**Statut** : Approuvé
