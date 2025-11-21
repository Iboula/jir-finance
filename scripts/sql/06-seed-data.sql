-- ============================================
-- Données de seed pour le développement et les tests
-- ============================================

-- Seed Users (mots de passe: tous "Password123!")
-- Hash BCrypt pour "Password123!": $2a$11$XxXxXxXxXxXxXxXxXxXxXxXxXxXxXxXxXxXxXxXxXxXxXx
INSERT INTO users (username, email, password_hash, prenom, nom, role, is_active) VALUES
('admin', 'admin@jir.local', '$2a$11$LKw8V9L9L8V9L8V9L8V9LuXxXxXxXxXxXxXxXxXxXxXxXxXxXxXxXxXxXx', 'Admin', 'Système', 'Admin', TRUE),
('manager1', 'manager1@jir.local', '$2a$11$LKw8V9L9L8V9L8V9L8V9LuXxXxXxXxXxXxXxXxXxXxXxXxXxXxXxXxXxXx', 'Jean', 'Dupont', 'Manager', TRUE),
('manager2', 'manager2@jir.local', '$2a$11$LKw8V9L9L8V9L8V9L8V9LuXxXxXxXxXxXxXxXxXxXxXxXxXxXxXxXxXxXx', 'Marie', 'Martin', 'Manager', TRUE),
('user1', 'user1@jir.local', '$2a$11$LKw8V9L9L8V9L8V9L8V9LuXxXxXxXxXxXxXxXxXxXxXxXxXxXxXxXxXxXx', 'Pierre', 'Dubois', 'User', TRUE),
('user2', 'user2@jir.local', '$2a$11$LKw8V9L9L8V9L8V9L8V9LuXxXxXxXxXxXxXxXxXxXxXxXxXxXxXxXxXxXx', 'Sophie', 'Bernard', 'User', TRUE),
('viewer', 'viewer@jir.local', '$2a$11$LKw8V9L9L8V9L8V9L8V9LuXxXxXxXxXxXxXxXxXxXxXxXxXxXxXxXxXxXx', 'Luc', 'Viewer', 'Viewer', TRUE);

-- Seed Sections
INSERT INTO sections (code, nom, description, responsable_id, budget_annuel, created_by) VALUES
('SEC001', 'Administration', 'Section administrative centrale', 2, 5000000.00, 'admin'),
('SEC002', 'Finances', 'Section de gestion financière', 2, 8000000.00, 'admin'),
('SEC003', 'Logistique', 'Section logistique et approvisionnement', 3, 3000000.00, 'admin'),
('SEC004', 'Formation', 'Section formation et développement', 3, 2000000.00, 'admin');

-- Mettre à jour section_id des users
UPDATE users SET section_id = 1 WHERE username = 'manager1';
UPDATE users SET section_id = 2 WHERE username = 'manager2';
UPDATE users SET section_id = 1 WHERE username = 'user1';
UPDATE users SET section_id = 2 WHERE username = 'user2';

-- Seed Partenaires
INSERT INTO partenaires (code, nom, type_partenaire, adresse, telephone, email, contact_nom, created_by) VALUES
('PART001', 'Fournisseur ABC', 'Fournisseur', '123 Rue du Commerce, Ville', '+225 01 02 03 04 05', 'contact@abc.com', 'M. Kouadio', 'admin'),
('PART002', 'Sponsor XYZ Corp', 'Sponsor', '456 Avenue des Sponsors', '+225 02 03 04 05 06', 'info@xyz.com', 'Mme Traoré', 'admin'),
('PART003', 'Client Beta', 'Client', '789 Boulevard Client', '+225 03 04 05 06 07', 'beta@client.com', 'M. Koné', 'admin'),
('PART004', 'Papeterie Moderne', 'Fournisseur', 'Zone Industrielle', '+225 04 05 06 07 08', 'papeterie@moderne.ci', 'Mme Diabaté', 'admin'),
('PART005', 'ONG Solidarité', 'Sponsor', 'Plateau', '+225 05 06 07 08 09', 'ong@solidarite.org', 'M. Bamba', 'admin');

-- Seed Magasins
INSERT INTO magasins (code, nom, localisation, responsable_id, created_by) VALUES
('MAG001', 'Magasin Central', 'Bâtiment A, Rez-de-chaussée', 4, 'admin'),
('MAG002', 'Entrepôt Annexe', 'Zone de stockage extérieure', 5, 'admin');

-- Seed Articles Magasin
INSERT INTO articles_magasin (magasin_id, code_article, designation, unite, quantite_stock, seuil_alerte, prix_unitaire) VALUES
(1, 'ART001', 'Ramettes de papier A4', 'Unité', 50, 10, 2500.00),
(1, 'ART002', 'Stylos bleus', 'Boîte', 20, 5, 1500.00),
(1, 'ART003', 'Agrafeuses', 'Unité', 15, 3, 3000.00),
(1, 'ART004', 'Classeurs', 'Unité', 30, 10, 1200.00),
(1, 'ART005', 'Marqueurs', 'Boîte', 8, 5, 2000.00),
(1, 'ART006', 'Chemises cartonnées', 'Paquet', 12, 5, 800.00),
(2, 'ART007', 'Tables pliantes', 'Unité', 25, 5, 25000.00),
(2, 'ART008', 'Chaises', 'Unité', 40, 10, 15000.00),
(2, 'ART009', 'Écrans projecteur', 'Unité', 3, 1, 150000.00),
(2, 'ART010', 'Câbles HDMI', 'Unité', 10, 3, 5000.00);

-- Seed Cotisations (derniers 3 mois)
INSERT INTO cotisations (section_id, membre_nom, membre_matricule, montant, periode, date_paiement, mode_paiement, recu_numero, statut, created_by) VALUES
-- Octobre 2025
(1, 'Kouassi Yao', 'MEM001', 5000.00, '2025-10', '2025-10-05', 'Espèces', NULL, 'Payé', 'manager1'),
(1, 'Aya Koffi', 'MEM002', 5000.00, '2025-10', '2025-10-06', 'Mobile Money', NULL, 'Payé', 'manager1'),
(2, 'Koffi Adjoua', 'MEM003', 7500.00, '2025-10', '2025-10-07', 'Virement', NULL, 'Payé', 'manager2'),
(2, 'Brou Jean', 'MEM004', 7500.00, '2025-10', '2025-10-08', 'Espèces', NULL, 'Payé', 'manager2'),
(3, 'Diabaté Marie', 'MEM005', 4000.00, '2025-10', '2025-10-10', 'Mobile Money', NULL, 'Payé', 'manager2'),
-- Novembre 2025
(1, 'Kouassi Yao', 'MEM001', 5000.00, '2025-11', '2025-11-03', 'Espèces', NULL, 'Payé', 'manager1'),
(1, 'Aya Koffi', 'MEM002', 5000.00, '2025-11', '2025-11-04', 'Mobile Money', NULL, 'Payé', 'manager1'),
(2, 'Koffi Adjoua', 'MEM003', 7500.00, '2025-11', '2025-11-05', 'Virement', NULL, 'Payé', 'manager2'),
(2, 'Brou Jean', 'MEM004', 7500.00, '2025-11', '2025-11-06', 'Espèces', NULL, 'Payé', 'manager2'),
(3, 'Diabaté Marie', 'MEM005', 4000.00, '2025-11', '2025-11-08', 'Chèque', NULL, 'Payé', 'manager2'),
(4, 'N''Guessan Paul', 'MEM006', 3000.00, '2025-11', '2025-11-10', 'Espèces', NULL, 'Payé', 'user1'),
(4, 'Touré Aminata', 'MEM007', 3000.00, '2025-11', '2025-11-12', 'Mobile Money', NULL, 'Payé', 'user1');

-- Seed Recettes
INSERT INTO recettes (section_id, libelle, montant, date_recette, source, categorie, mode_encaissement, created_by) VALUES
(1, 'Subvention annuelle gouvernement', 2000000.00, '2025-01-15', 'Gouvernement', 'Subvention', 'Virement', 'manager1'),
(2, 'Don ONG Solidarité', 500000.00, '2025-03-20', 'ONG Solidarité', 'Don', 'Virement', 'manager2'),
(3, 'Vente matériel obsolète', 150000.00, '2025-09-10', 'Vente interne', 'Vente', 'Espèces', 'manager2'),
(1, 'Contribution externe partenaire', 300000.00, '2025-10-05', 'Sponsor XYZ Corp', 'Partenariat', 'Virement', 'manager1'),
(4, 'Frais de formation externe', 75000.00, '2025-11-01', 'Participants externes', 'Prestation', 'Mobile Money', 'user1');

-- Seed Dépenses
INSERT INTO depenses (section_id, libelle, montant, date_depense, beneficiaire, categorie, mode_paiement, statut_workflow, demandeur_id, validateur_1_id, validateur_2_id, created_by) VALUES
-- Dépenses payées
(1, 'Achat fournitures de bureau', 125000.00, '2025-10-10', 'Papeterie Moderne', 'Fonctionnement', 'Virement', 'Payé', 4, 2, 1, 'user1'),
(2, 'Frais de mission à l''étranger', 850000.00, '2025-10-15', 'Jean Dupont', 'Fonctionnement', 'Virement', 'Payé', 5, 3, 1, 'user2'),
(3, 'Achat mobilier de bureau', 450000.00, '2025-11-01', 'Fournisseur ABC', 'Investissement', 'Chèque', 'Payé', 4, 3, 1, 'user1'),
-- Dépenses validées mais non payées
(1, 'Location salle de conférence', 200000.00, '2025-11-10', 'Hôtel Ivoire', 'Fonctionnement', 'Virement', 'Validé', 4, 2, 1, 'user1'),
-- Dépenses en attente de validation
(2, 'Formation du personnel', 350000.00, '2025-11-15', 'Centre de Formation Pro', 'Formation', 'Virement', 'En attente validation', 5, 3, NULL, 'user2'),
(4, 'Achat équipement informatique', 1200000.00, '2025-11-16', 'Informatique Plus', 'Investissement', 'Virement', 'En attente validation', 4, 2, NULL, 'user1'),
-- Dépenses en brouillon
(3, 'Réparation véhicule de service', 180000.00, '2025-11-17', 'Garage Central', 'Fonctionnement', NULL, 'Brouillon', 5, NULL, NULL, 'user2');

-- Seed Mouvements Stock (Entrées)
INSERT INTO mouvements_stock (article_id, type_mouvement, quantite, date_mouvement, numero_document, motif, created_by) VALUES
(1, 'Entrée', 50.000, '2025-09-01', 'BE-2025-001', 'Achat initial stock', 'user1'),
(2, 'Entrée', 20.000, '2025-09-01', 'BE-2025-001', 'Achat initial stock', 'user1'),
(3, 'Entrée', 15.000, '2025-09-01', 'BE-2025-001', 'Achat initial stock', 'user1'),
(4, 'Entrée', 30.000, '2025-09-01', 'BE-2025-001', 'Achat initial stock', 'user1'),
(5, 'Entrée', 15.000, '2025-10-15', 'BE-2025-002', 'Réapprovisionnement', 'user1'),
(6, 'Entrée', 20.000, '2025-10-15', 'BE-2025-002', 'Réapprovisionnement', 'user1');

-- Seed Mouvements Stock (Sorties) - pour créer des alertes
INSERT INTO mouvements_stock (article_id, type_mouvement, quantite, date_mouvement, numero_document, motif, created_by) VALUES
(5, 'Sortie', 7.000, '2025-11-10', 'BS-2025-001', 'Fourniture section Admin', 'user1'),
(6, 'Sortie', 8.000, '2025-11-12', 'BS-2025-002', 'Fourniture section Finances', 'user1');

-- Afficher un message de succès
DO $$
BEGIN
    RAISE NOTICE '✅ Données de seed insérées avec succès!';
    RAISE NOTICE '📊 Résumé:';
    RAISE NOTICE '   - % utilisateurs créés', (SELECT COUNT(*) FROM users);
    RAISE NOTICE '   - % sections créées', (SELECT COUNT(*) FROM sections);
    RAISE NOTICE '   - % partenaires créés', (SELECT COUNT(*) FROM partenaires);
    RAISE NOTICE '   - % magasins créés', (SELECT COUNT(*) FROM magasins);
    RAISE NOTICE '   - % articles créés', (SELECT COUNT(*) FROM articles_magasin);
    RAISE NOTICE '   - % cotisations créées', (SELECT COUNT(*) FROM cotisations);
    RAISE NOTICE '   - % recettes créées', (SELECT COUNT(*) FROM recettes);
    RAISE NOTICE '   - % dépenses créées', (SELECT COUNT(*) FROM depenses);
    RAISE NOTICE '   - % mouvements de stock créés', (SELECT COUNT(*) FROM mouvements_stock);
    RAISE NOTICE '';
    RAISE NOTICE '🔑 Comptes de test (mot de passe pour tous: Password123!):';
    RAISE NOTICE '   - admin / Admin (Administrateur)';
    RAISE NOTICE '   - manager1 / Manager1 (Manager Section Administration)';
    RAISE NOTICE '   - manager2 / Manager2 (Manager Section Finances)';
    RAISE NOTICE '   - user1 / User1 (Utilisateur)';
    RAISE NOTICE '   - user2 / User2 (Utilisateur)';
    RAISE NOTICE '   - viewer / Viewer (Lecteur seul)';
END $$;
