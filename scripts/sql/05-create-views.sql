-- ============================================
-- Création des vues pour les rapports
-- ============================================

-- Vue: Solde de caisse par section
CREATE OR REPLACE VIEW v_solde_caisse_par_section AS
SELECT 
    s.id as section_id,
    s.code as section_code,
    s.nom as section_nom,
    COALESCE(SUM(c.montant), 0) as total_cotisations,
    COALESCE(SUM(r.montant), 0) as total_recettes,
    COALESCE(SUM(d.montant), 0) as total_depenses,
    (COALESCE(SUM(c.montant), 0) + COALESCE(SUM(r.montant), 0) - COALESCE(SUM(d.montant), 0)) as solde_caisse,
    COUNT(DISTINCT c.id) as nombre_cotisations,
    COUNT(DISTINCT r.id) as nombre_recettes,
    COUNT(DISTINCT d.id) as nombre_depenses
FROM sections s
LEFT JOIN cotisations c ON s.id = c.section_id 
    AND c.is_deleted = FALSE 
    AND c.statut = 'Payé'
LEFT JOIN recettes r ON s.id = r.section_id 
    AND r.is_deleted = FALSE
LEFT JOIN depenses d ON s.id = d.section_id 
    AND d.is_deleted = FALSE 
    AND d.statut_workflow = 'Payé'
WHERE s.is_deleted = FALSE
GROUP BY s.id, s.code, s.nom
ORDER BY s.nom;

COMMENT ON VIEW v_solde_caisse_par_section IS 'Solde de caisse calculé pour chaque section';

-- Vue: Solde de caisse global
CREATE OR REPLACE VIEW v_solde_caisse_global AS
SELECT 
    SUM(total_cotisations) as total_cotisations_global,
    SUM(total_recettes) as total_recettes_global,
    SUM(total_depenses) as total_depenses_global,
    SUM(solde_caisse) as solde_caisse_global,
    COUNT(*) as nombre_sections_actives
FROM v_solde_caisse_par_section;

COMMENT ON VIEW v_solde_caisse_global IS 'Solde de caisse global toutes sections confondues';

-- Vue: Dépenses en attente de validation
CREATE OR REPLACE VIEW v_depenses_en_attente_validation AS
SELECT 
    d.id,
    d.numero_bon,
    d.libelle,
    d.montant,
    d.date_depense,
    d.beneficiaire,
    d.statut_workflow,
    s.nom as section_nom,
    s.code as section_code,
    u1.username as demandeur_nom,
    u1.prenom || ' ' || u1.nom as demandeur_complet,
    u2.username as validateur_1_nom,
    u3.username as validateur_2_nom,
    d.date_validation_1,
    d.date_validation_2,
    d.created_at as date_creation,
    EXTRACT(DAY FROM (CURRENT_TIMESTAMP - d.created_at)) as jours_en_attente
FROM depenses d
JOIN sections s ON d.section_id = s.id
LEFT JOIN users u1 ON d.demandeur_id = u1.id
LEFT JOIN users u2 ON d.validateur_1_id = u2.id
LEFT JOIN users u3 ON d.validateur_2_id = u3.id
WHERE d.statut_workflow IN ('Brouillon', 'En attente validation')
  AND d.is_deleted = FALSE
ORDER BY d.created_at DESC;

COMMENT ON VIEW v_depenses_en_attente_validation IS 'Liste des dépenses en attente de validation avec détails';

-- Vue: Articles en alerte de stock
CREATE OR REPLACE VIEW v_articles_stock_alerte AS
SELECT 
    a.id,
    a.code_article,
    a.designation,
    a.unite,
    a.quantite_stock,
    a.seuil_alerte,
    (a.quantite_stock - a.seuil_alerte) as ecart_seuil,
    m.nom as magasin_nom,
    m.code as magasin_code,
    u.username as responsable_magasin,
    CASE 
        WHEN a.quantite_stock = 0 THEN 'Rupture de stock'
        WHEN a.quantite_stock <= a.seuil_alerte * 0.5 THEN 'Critique'
        WHEN a.quantite_stock <= a.seuil_alerte THEN 'Alerte'
        ELSE 'Normal'
    END as niveau_alerte
FROM articles_magasin a
JOIN magasins m ON a.magasin_id = m.id
LEFT JOIN users u ON m.responsable_id = u.id
WHERE a.quantite_stock <= a.seuil_alerte
  AND m.is_deleted = FALSE
ORDER BY 
    CASE 
        WHEN a.quantite_stock = 0 THEN 1
        WHEN a.quantite_stock <= a.seuil_alerte * 0.5 THEN 2
        ELSE 3
    END,
    a.quantite_stock ASC;

COMMENT ON VIEW v_articles_stock_alerte IS 'Articles dont le stock est sous le seuil d''alerte';

-- Vue: Cotisations par période
CREATE OR REPLACE VIEW v_cotisations_par_periode AS
SELECT 
    c.periode,
    s.id as section_id,
    s.nom as section_nom,
    COUNT(c.id) as nombre_cotisations,
    SUM(c.montant) as montant_total,
    AVG(c.montant) as montant_moyen,
    MIN(c.montant) as montant_min,
    MAX(c.montant) as montant_max,
    COUNT(DISTINCT c.membre_matricule) as nombre_membres_uniques
FROM cotisations c
JOIN sections s ON c.section_id = s.id
WHERE c.is_deleted = FALSE
  AND c.statut = 'Payé'
GROUP BY c.periode, s.id, s.nom
ORDER BY c.periode DESC, s.nom;

COMMENT ON VIEW v_cotisations_par_periode IS 'Statistiques des cotisations agrégées par période et section';

-- Vue: Dépenses par catégorie
CREATE OR REPLACE VIEW v_depenses_par_categorie AS
SELECT 
    d.categorie,
    s.id as section_id,
    s.nom as section_nom,
    COUNT(d.id) as nombre_depenses,
    SUM(d.montant) as montant_total,
    AVG(d.montant) as montant_moyen,
    MIN(d.date_depense) as premiere_depense,
    MAX(d.date_depense) as derniere_depense
FROM depenses d
JOIN sections s ON d.section_id = s.id
WHERE d.is_deleted = FALSE
  AND d.statut_workflow IN ('Validé', 'Payé')
GROUP BY d.categorie, s.id, s.nom
ORDER BY montant_total DESC;

COMMENT ON VIEW v_depenses_par_categorie IS 'Statistiques des dépenses par catégorie';

-- Vue: Mouvements de stock récents
CREATE OR REPLACE VIEW v_mouvements_stock_recents AS
SELECT 
    ms.id,
    ms.type_mouvement,
    ms.quantite,
    ms.date_mouvement,
    ms.numero_document,
    ms.motif,
    ms.created_by,
    a.code_article,
    a.designation as article_designation,
    a.unite,
    a.quantite_stock as stock_actuel,
    m.nom as magasin_nom,
    m.code as magasin_code
FROM mouvements_stock ms
JOIN articles_magasin a ON ms.article_id = a.id
JOIN magasins m ON a.magasin_id = m.id
WHERE m.is_deleted = FALSE
ORDER BY ms.date_mouvement DESC
LIMIT 100;

COMMENT ON VIEW v_mouvements_stock_recents IS '100 derniers mouvements de stock';

-- Vue: Dashboard - Statistiques mensuelles
CREATE OR REPLACE VIEW v_dashboard_stats_mensuel AS
WITH mois_courant AS (
    SELECT 
        TO_CHAR(CURRENT_DATE, 'YYYY-MM') as periode
)
SELECT 
    mc.periode,
    COALESCE(SUM(c.montant), 0) as cotisations_mois,
    COALESCE(SUM(r.montant), 0) as recettes_mois,
    COALESCE(SUM(d.montant), 0) as depenses_mois,
    COUNT(DISTINCT c.id) as nb_cotisations,
    COUNT(DISTINCT r.id) as nb_recettes,
    COUNT(DISTINCT d.id) as nb_depenses,
    (SELECT COUNT(*) FROM v_depenses_en_attente_validation) as nb_depenses_en_attente,
    (SELECT COUNT(*) FROM v_articles_stock_alerte) as nb_articles_alerte
FROM mois_courant mc
LEFT JOIN cotisations c ON c.periode = mc.periode 
    AND c.statut = 'Payé' 
    AND c.is_deleted = FALSE
LEFT JOIN recettes r ON TO_CHAR(r.date_recette, 'YYYY-MM') = mc.periode 
    AND r.is_deleted = FALSE
LEFT JOIN depenses d ON TO_CHAR(d.date_depense, 'YYYY-MM') = mc.periode 
    AND d.statut_workflow = 'Payé' 
    AND d.is_deleted = FALSE
GROUP BY mc.periode;

COMMENT ON VIEW v_dashboard_stats_mensuel IS 'Statistiques pour le dashboard du mois en cours';
