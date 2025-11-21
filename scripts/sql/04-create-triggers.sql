-- ============================================
-- Création des fonctions et triggers
-- ============================================

-- Fonction pour mettre à jour automatiquement updated_at
CREATE OR REPLACE FUNCTION update_timestamp()
RETURNS TRIGGER AS $$
BEGIN
    NEW.updated_at = CURRENT_TIMESTAMP;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- Appliquer le trigger updated_at sur toutes les tables concernées
CREATE TRIGGER trg_users_updated_at 
    BEFORE UPDATE ON users
    FOR EACH ROW EXECUTE FUNCTION update_timestamp();

CREATE TRIGGER trg_sections_updated_at 
    BEFORE UPDATE ON sections
    FOR EACH ROW EXECUTE FUNCTION update_timestamp();

CREATE TRIGGER trg_cotisations_updated_at 
    BEFORE UPDATE ON cotisations
    FOR EACH ROW EXECUTE FUNCTION update_timestamp();

CREATE TRIGGER trg_depenses_updated_at 
    BEFORE UPDATE ON depenses
    FOR EACH ROW EXECUTE FUNCTION update_timestamp();

CREATE TRIGGER trg_recettes_updated_at 
    BEFORE UPDATE ON recettes
    FOR EACH ROW EXECUTE FUNCTION update_timestamp();

CREATE TRIGGER trg_partenaires_updated_at 
    BEFORE UPDATE ON partenaires
    FOR EACH ROW EXECUTE FUNCTION update_timestamp();

CREATE TRIGGER trg_magasins_updated_at 
    BEFORE UPDATE ON magasins
    FOR EACH ROW EXECUTE FUNCTION update_timestamp();

CREATE TRIGGER trg_articles_updated_at 
    BEFORE UPDATE ON articles_magasin
    FOR EACH ROW EXECUTE FUNCTION update_timestamp();

-- Fonction pour mettre à jour le stock automatiquement après un mouvement
CREATE OR REPLACE FUNCTION update_stock_after_mouvement()
RETURNS TRIGGER AS $$
BEGIN
    IF NEW.type_mouvement = 'Entrée' THEN
        UPDATE articles_magasin 
        SET quantite_stock = quantite_stock + NEW.quantite,
            updated_at = CURRENT_TIMESTAMP
        WHERE id = NEW.article_id;
    ELSIF NEW.type_mouvement = 'Sortie' THEN
        UPDATE articles_magasin 
        SET quantite_stock = quantite_stock - NEW.quantite,
            updated_at = CURRENT_TIMESTAMP
        WHERE id = NEW.article_id;
        
        -- Vérifier que le stock ne devient pas négatif
        IF (SELECT quantite_stock FROM articles_magasin WHERE id = NEW.article_id) < 0 THEN
            RAISE EXCEPTION 'Stock insuffisant pour l''article ID %', NEW.article_id;
        END IF;
    END IF;
    
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_update_stock 
    AFTER INSERT ON mouvements_stock
    FOR EACH ROW EXECUTE FUNCTION update_stock_after_mouvement();

-- Fonction pour générer automatiquement un numéro de reçu de cotisation
CREATE OR REPLACE FUNCTION generate_recu_numero_cotisation()
RETURNS TRIGGER AS $$
BEGIN
    IF NEW.recu_numero IS NULL THEN
        NEW.recu_numero := 'COT-' || TO_CHAR(NEW.date_paiement, 'YYYYMM') || '-' || LPAD(NEW.id::TEXT, 6, '0');
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_generate_recu_cotisation 
    BEFORE INSERT ON cotisations
    FOR EACH ROW EXECUTE FUNCTION generate_recu_numero_cotisation();

-- Fonction pour générer automatiquement un numéro de bon de dépense
CREATE OR REPLACE FUNCTION generate_numero_bon_depense()
RETURNS TRIGGER AS $$
BEGIN
    IF NEW.numero_bon IS NULL OR NEW.numero_bon = '' THEN
        NEW.numero_bon := 'DEP-' || TO_CHAR(NEW.date_depense, 'YYYYMM') || '-' || LPAD(NEW.id::TEXT, 6, '0');
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_generate_numero_bon 
    BEFORE INSERT ON depenses
    FOR EACH ROW EXECUTE FUNCTION generate_numero_bon_depense();

-- Fonction pour générer automatiquement un numéro de reçu de recette
CREATE OR REPLACE FUNCTION generate_recu_numero_recette()
RETURNS TRIGGER AS $$
BEGIN
    IF NEW.numero_recu IS NULL OR NEW.numero_recu = '' THEN
        NEW.numero_recu := 'REC-' || TO_CHAR(NEW.date_recette, 'YYYYMM') || '-' || LPAD(NEW.id::TEXT, 6, '0');
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_generate_recu_recette 
    BEFORE INSERT ON recettes
    FOR EACH ROW EXECUTE FUNCTION generate_recu_numero_recette();

-- Fonction pour logger les changements de statut des dépenses (optionnel - audit)
CREATE TABLE IF NOT EXISTS depenses_audit_log (
    id BIGSERIAL PRIMARY KEY,
    depense_id BIGINT NOT NULL,
    ancien_statut VARCHAR(50),
    nouveau_statut VARCHAR(50),
    user_id BIGINT,
    changed_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    commentaire TEXT
);

CREATE OR REPLACE FUNCTION log_depense_status_change()
RETURNS TRIGGER AS $$
BEGIN
    IF OLD.statut_workflow IS DISTINCT FROM NEW.statut_workflow THEN
        INSERT INTO depenses_audit_log (depense_id, ancien_statut, nouveau_statut, commentaire)
        VALUES (NEW.id, OLD.statut_workflow, NEW.statut_workflow, 
                'Changement de statut: ' || OLD.statut_workflow || ' -> ' || NEW.statut_workflow);
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_log_depense_status 
    AFTER UPDATE ON depenses
    FOR EACH ROW 
    WHEN (OLD.statut_workflow IS DISTINCT FROM NEW.statut_workflow)
    EXECUTE FUNCTION log_depense_status_change();
