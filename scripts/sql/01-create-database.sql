-- ============================================
-- Script de création de la base de données JIR
-- Système de Gestion Financière
-- Date: 2025-11-17
-- ============================================

-- Créer la base de données
CREATE DATABASE jir_finance
    WITH 
    ENCODING = 'UTF8'
    LC_COLLATE = 'fr_FR.UTF-8'
    LC_CTYPE = 'fr_FR.UTF-8'
    TEMPLATE = template0;

-- Se connecter à la base
\c jir_finance

-- Activer les extensions nécessaires
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
CREATE EXTENSION IF NOT EXISTS "pg_trgm";      -- Pour recherche full-text
CREATE EXTENSION IF NOT EXISTS "unaccent";    -- Pour recherche sans accents

-- Définir le schéma par défaut
SET search_path TO public;

-- Commenter la base
COMMENT ON DATABASE jir_finance IS 'Base de données du système de gestion financière JIR';
