-- ============================================
-- Création des tables principales
-- Système de Gestion Financière JIR
-- ============================================

-- Table: users (doit être créée en premier car référencée par d'autres tables)
CREATE TABLE users (
    id BIGSERIAL PRIMARY KEY,
    username VARCHAR(100) UNIQUE NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    password_hash VARCHAR(500) NOT NULL,
    prenom VARCHAR(100),
    nom VARCHAR(100),
    role VARCHAR(50) NOT NULL CHECK (role IN ('Admin', 'Manager', 'User', 'Viewer')),
    section_id BIGINT,
    is_active BOOLEAN DEFAULT TRUE,
    last_login TIMESTAMP WITH TIME ZONE,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

COMMENT ON TABLE users IS 'Utilisateurs du système avec authentification et rôles';
COMMENT ON COLUMN users.role IS 'Admin: accès total, Manager: gestion section + validation, User: création, Viewer: lecture seule';

-- Table: sections
CREATE TABLE sections (
    id BIGSERIAL PRIMARY KEY,
    code VARCHAR(50) UNIQUE NOT NULL,
    nom VARCHAR(200) NOT NULL,
    description TEXT,
    responsable_id BIGINT REFERENCES users(id) ON DELETE SET NULL,
    budget_annuel DECIMAL(18,2) DEFAULT 0,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    created_by VARCHAR(100),
    updated_by VARCHAR(100),
    is_deleted BOOLEAN DEFAULT FALSE
);

COMMENT ON TABLE sections IS 'Sections de l''organisation';
COMMENT ON COLUMN sections.budget_annuel IS 'Budget annuel alloué en FCFA';

-- Ajouter la FK section_id pour users (après création de sections)
ALTER TABLE users 
ADD CONSTRAINT fk_users_section 
FOREIGN KEY (section_id) REFERENCES sections(id) ON DELETE SET NULL;

-- Table: cotisations
CREATE TABLE cotisations (
    id BIGSERIAL PRIMARY KEY,
    section_id BIGINT NOT NULL REFERENCES sections(id) ON DELETE RESTRICT,
    membre_nom VARCHAR(200) NOT NULL,
    membre_matricule VARCHAR(50) NOT NULL,
    montant DECIMAL(18,2) NOT NULL CHECK (montant > 0),
    periode VARCHAR(7) NOT NULL CHECK (periode ~ '^[0-9]{4}-[0-9]{2}$'),
    date_paiement DATE NOT NULL,
    mode_paiement VARCHAR(50) NOT NULL CHECK (mode_paiement IN ('Espèces', 'Virement', 'Chèque', 'Mobile Money')),
    recu_numero VARCHAR(100) UNIQUE,
    statut VARCHAR(20) DEFAULT 'Payé' CHECK (statut IN ('Payé', 'En attente', 'Annulé')),
    notes TEXT,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    created_by VARCHAR(100),
    updated_by VARCHAR(100),
    is_deleted BOOLEAN DEFAULT FALSE
);

COMMENT ON TABLE cotisations IS 'Cotisations des membres par section';
COMMENT ON COLUMN cotisations.periode IS 'Format YYYY-MM (ex: 2025-11)';
COMMENT ON COLUMN cotisations.recu_numero IS 'Numéro unique du reçu généré automatiquement';

-- Table: depenses
CREATE TABLE depenses (
    id BIGSERIAL PRIMARY KEY,
    section_id BIGINT NOT NULL REFERENCES sections(id) ON DELETE RESTRICT,
    numero_bon VARCHAR(100) UNIQUE NOT NULL,
    libelle VARCHAR(500) NOT NULL,
    montant DECIMAL(18,2) NOT NULL CHECK (montant > 0),
    date_depense DATE NOT NULL,
    beneficiaire VARCHAR(200) NOT NULL,
    categorie VARCHAR(100) NOT NULL,
    mode_paiement VARCHAR(50) CHECK (mode_paiement IN ('Espèces', 'Virement', 'Chèque')),
    statut_workflow VARCHAR(50) DEFAULT 'Brouillon' CHECK (statut_workflow IN ('Brouillon', 'En attente validation', 'Validé', 'Rejeté', 'Payé')),
    demandeur_id BIGINT REFERENCES users(id) ON DELETE SET NULL,
    validateur_1_id BIGINT REFERENCES users(id) ON DELETE SET NULL,
    validateur_2_id BIGINT REFERENCES users(id) ON DELETE SET NULL,
    date_validation_1 TIMESTAMP WITH TIME ZONE,
    date_validation_2 TIMESTAMP WITH TIME ZONE,
    signature_1_path VARCHAR(500),
    signature_2_path VARCHAR(500),
    justificatif_path VARCHAR(500),
    motif_rejet TEXT,
    notes TEXT,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    created_by VARCHAR(100),
    updated_by VARCHAR(100),
    is_deleted BOOLEAN DEFAULT FALSE
);

COMMENT ON TABLE depenses IS 'Dépenses avec workflow de validation à signatures électroniques';
COMMENT ON COLUMN depenses.statut_workflow IS 'Workflow: Brouillon -> En attente -> Validé -> Payé (ou Rejeté)';
COMMENT ON COLUMN depenses.signature_1_path IS 'Chemin vers l''image de signature du validateur 1';

-- Table: recettes
CREATE TABLE recettes (
    id BIGSERIAL PRIMARY KEY,
    section_id BIGINT NOT NULL REFERENCES sections(id) ON DELETE RESTRICT,
    numero_recu VARCHAR(100) UNIQUE NOT NULL,
    libelle VARCHAR(500) NOT NULL,
    montant DECIMAL(18,2) NOT NULL CHECK (montant > 0),
    date_recette DATE NOT NULL,
    source VARCHAR(200) NOT NULL,
    categorie VARCHAR(100) NOT NULL,
    mode_encaissement VARCHAR(50) NOT NULL CHECK (mode_encaissement IN ('Espèces', 'Virement', 'Chèque', 'Mobile Money')),
    notes TEXT,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    created_by VARCHAR(100),
    updated_by VARCHAR(100),
    is_deleted BOOLEAN DEFAULT FALSE
);

COMMENT ON TABLE recettes IS 'Recettes hors cotisations (subventions, dons, ventes, etc.)';

-- Table: partenaires
CREATE TABLE partenaires (
    id BIGSERIAL PRIMARY KEY,
    code VARCHAR(50) UNIQUE NOT NULL,
    nom VARCHAR(200) NOT NULL,
    type_partenaire VARCHAR(50) CHECK (type_partenaire IN ('Fournisseur', 'Client', 'Sponsor', 'Autre')),
    adresse TEXT,
    telephone VARCHAR(50),
    email VARCHAR(100),
    contact_nom VARCHAR(200),
    notes TEXT,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    created_by VARCHAR(100),
    updated_by VARCHAR(100),
    is_deleted BOOLEAN DEFAULT FALSE
);

COMMENT ON TABLE partenaires IS 'Annuaire des partenaires (fournisseurs, clients, sponsors)';

-- Table: magasins
CREATE TABLE magasins (
    id BIGSERIAL PRIMARY KEY,
    code VARCHAR(50) UNIQUE NOT NULL,
    nom VARCHAR(200) NOT NULL,
    localisation VARCHAR(200),
    responsable_id BIGINT REFERENCES users(id) ON DELETE SET NULL,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    created_by VARCHAR(100),
    updated_by VARCHAR(100),
    is_deleted BOOLEAN DEFAULT FALSE
);

COMMENT ON TABLE magasins IS 'Magasins/entrepôts pour gestion des stocks';

-- Table: articles_magasin
CREATE TABLE articles_magasin (
    id BIGSERIAL PRIMARY KEY,
    magasin_id BIGINT NOT NULL REFERENCES magasins(id) ON DELETE CASCADE,
    code_article VARCHAR(100) UNIQUE NOT NULL,
    designation VARCHAR(300) NOT NULL,
    unite VARCHAR(50) NOT NULL,
    quantite_stock DECIMAL(18,3) DEFAULT 0 CHECK (quantite_stock >= 0),
    seuil_alerte DECIMAL(18,3) DEFAULT 10,
    prix_unitaire DECIMAL(18,2),
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

COMMENT ON TABLE articles_magasin IS 'Articles en stock dans les magasins';
COMMENT ON COLUMN articles_magasin.unite IS 'Unité de mesure: Kg, L, Unité, Boîte, etc.';
COMMENT ON COLUMN articles_magasin.seuil_alerte IS 'Seuil pour alerte stock faible';

-- Table: mouvements_stock
CREATE TABLE mouvements_stock (
    id BIGSERIAL PRIMARY KEY,
    article_id BIGINT NOT NULL REFERENCES articles_magasin(id) ON DELETE RESTRICT,
    type_mouvement VARCHAR(20) NOT NULL CHECK (type_mouvement IN ('Entrée', 'Sortie')),
    quantite DECIMAL(18,3) NOT NULL CHECK (quantite > 0),
    date_mouvement TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    numero_document VARCHAR(100),
    motif TEXT,
    created_by VARCHAR(100),
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

COMMENT ON TABLE mouvements_stock IS 'Historique des mouvements de stock (entrées/sorties)';
COMMENT ON COLUMN mouvements_stock.numero_document IS 'Numéro du bon d''entrée ou de sortie';
