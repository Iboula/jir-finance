-- ============================================
-- Création des index pour optimisation des performances
-- ============================================

-- Index sur users
CREATE INDEX idx_users_username ON users(username);
CREATE INDEX idx_users_email ON users(email);
CREATE INDEX idx_users_role ON users(role);
CREATE INDEX idx_users_section ON users(section_id);
CREATE INDEX idx_users_active ON users(is_active) WHERE is_active = TRUE;

-- Index sur sections
CREATE INDEX idx_sections_code ON sections(code);
CREATE INDEX idx_sections_responsable ON sections(responsable_id);
CREATE INDEX idx_sections_active ON sections(is_deleted) WHERE is_deleted = FALSE;

-- Index sur cotisations
CREATE INDEX idx_cotisations_section ON cotisations(section_id);
CREATE INDEX idx_cotisations_membre ON cotisations(membre_matricule);
CREATE INDEX idx_cotisations_periode ON cotisations(periode);
CREATE INDEX idx_cotisations_date ON cotisations(date_paiement);
CREATE INDEX idx_cotisations_statut ON cotisations(statut);
CREATE INDEX idx_cotisations_recu ON cotisations(recu_numero);
CREATE INDEX idx_cotisations_active ON cotisations(is_deleted) WHERE is_deleted = FALSE;

-- Index composé pour recherches fréquentes sur cotisations
CREATE INDEX idx_cotisations_section_periode ON cotisations(section_id, periode);

-- Index sur depenses
CREATE INDEX idx_depenses_section ON depenses(section_id);
CREATE INDEX idx_depenses_numero ON depenses(numero_bon);
CREATE INDEX idx_depenses_date ON depenses(date_depense);
CREATE INDEX idx_depenses_statut ON depenses(statut_workflow);
CREATE INDEX idx_depenses_demandeur ON depenses(demandeur_id);
CREATE INDEX idx_depenses_validateur1 ON depenses(validateur_1_id);
CREATE INDEX idx_depenses_validateur2 ON depenses(validateur_2_id);
CREATE INDEX idx_depenses_categorie ON depenses(categorie);
CREATE INDEX idx_depenses_active ON depenses(is_deleted) WHERE is_deleted = FALSE;

-- Index composé pour workflow
CREATE INDEX idx_depenses_statut_section ON depenses(statut_workflow, section_id);

-- Index sur recettes
CREATE INDEX idx_recettes_section ON recettes(section_id);
CREATE INDEX idx_recettes_numero ON recettes(numero_recu);
CREATE INDEX idx_recettes_date ON recettes(date_recette);
CREATE INDEX idx_recettes_categorie ON recettes(categorie);
CREATE INDEX idx_recettes_source ON recettes(source);
CREATE INDEX idx_recettes_active ON recettes(is_deleted) WHERE is_deleted = FALSE;

-- Index sur partenaires
CREATE INDEX idx_partenaires_code ON partenaires(code);
CREATE INDEX idx_partenaires_nom ON partenaires(nom);
CREATE INDEX idx_partenaires_type ON partenaires(type_partenaire);
CREATE INDEX idx_partenaires_active ON partenaires(is_deleted) WHERE is_deleted = FALSE;

-- Index pour recherche full-text sur partenaires
CREATE INDEX idx_partenaires_nom_trgm ON partenaires USING gin(nom gin_trgm_ops);

-- Index sur magasins
CREATE INDEX idx_magasins_code ON magasins(code);
CREATE INDEX idx_magasins_responsable ON magasins(responsable_id);
CREATE INDEX idx_magasins_active ON magasins(is_deleted) WHERE is_deleted = FALSE;

-- Index sur articles_magasin
CREATE INDEX idx_articles_magasin ON articles_magasin(magasin_id);
CREATE INDEX idx_articles_code ON articles_magasin(code_article);
CREATE INDEX idx_articles_designation ON articles_magasin(designation);
-- Index partiel pour articles en alerte de stock
CREATE INDEX idx_articles_stock_faible ON articles_magasin(quantite_stock) 
WHERE quantite_stock <= seuil_alerte;

-- Index sur mouvements_stock
CREATE INDEX idx_mouvements_article ON mouvements_stock(article_id);
CREATE INDEX idx_mouvements_date ON mouvements_stock(date_mouvement);
CREATE INDEX idx_mouvements_type ON mouvements_stock(type_mouvement);
CREATE INDEX idx_mouvements_numero ON mouvements_stock(numero_document);

-- Index composé pour historique
CREATE INDEX idx_mouvements_article_date ON mouvements_stock(article_id, date_mouvement DESC);
