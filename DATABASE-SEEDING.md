# Guide de Seeding de la Base de Données

## Vue d'ensemble

Le système de seeding initialise automatiquement la base de données avec des données de démonstration au démarrage de l'application. Cela se produit uniquement si la base de données est vide.

## Données générées

Le seeder crée automatiquement:

### 1. Magasins (17 magasins)
- Magasin Central, Succursale Nord, Succursale Sud, etc.
- Chaque magasin avec une adresse et un numéro de téléphone

### 2. Articles (5-10 par magasin)
- Produits technologiques variés (ordinateurs, accessoires, etc.)
- Prix aléatoires entre 10$ et 1000$
- Stock initial aléatoire (0-100 unités)
- Seuil d'alerte (5-20 unités)

### 3. Mouvements de Stock
- 2-5 mouvements pour environ 50% des articles
- Types: Entrée, Sortie, Transfert
- Dates sur les 90 derniers jours
- Commentaires descriptifs

### 4. Sections (8 sections)
- Direction, Comptabilité, RH, Ventes, Marketing, Logistique, IT, Service Client

### 5. Partenaires (6 partenaires)
- 2 Fournisseurs
- 2 Clients
- 2 Autres (Transporteur, Consultant)

### 6. Recettes (10 recettes)
- Montants entre 1000$ et 50000$
- Réparties sur les 90 derniers jours

### 7. Dépenses (15 dépenses)
- Types variés: Fournitures, Salaires, Loyer, etc.
- Montants entre 500$ et 10000$
- Différents statuts de workflow

### 8. Cotisations (10 cotisations)
- Montants entre 100$ et 1000$
- Derniers 30 jours

## Comment ça fonctionne

### Lors du démarrage de l'application

1. **Vérification**: Le système vérifie si la table `Magasins` contient des données
2. **Skip si existant**: Si des données existent, le seeding est ignoré
3. **Exécution si vide**: Si la base est vide, toutes les données sont créées

### Code dans Program.cs

```csharp
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    
    try
    {
        logger.LogInformation("Applying database migrations...");
        await context.Database.MigrateAsync();
        logger.LogInformation("Database migrations applied successfully.");
        
        logger.LogInformation("Seeding database...");
        await DatabaseSeeder.SeedAsync(context);
        logger.LogInformation("Database seeding completed successfully.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while migrating or seeding the database.");
        throw;
    }
}
```

## Tester le seeding localement

### Option 1: Nouvelle base de données

```powershell
# 1. Supprimer la base de données existante
docker-compose down -v

# 2. Redémarrer les conteneurs
docker-compose up -d

# 3. Vérifier les logs
docker logs jir-api -f
```

Vous devriez voir:
```
Applying database migrations...
Database migrations applied successfully.
Seeding database...
Database seeding completed successfully.
```

### Option 2: Base PostgreSQL locale

```powershell
# 1. Se connecter à PostgreSQL
psql -U postgres -d jir_finance

# 2. Supprimer toutes les tables
DROP SCHEMA public CASCADE;
CREATE SCHEMA public;

# 3. Quitter psql
\q

# 4. Relancer l'API
cd src/JIR.WebAPI
dotnet run
```

### Option 3: Tester avec une nouvelle base

```powershell
# Modifier appsettings.json temporairement
# ConnectionStrings:DefaultConnection = "Host=localhost;Port=5432;Database=jir_finance_test;Username=postgres"

# Créer la nouvelle base
psql -U postgres -c "CREATE DATABASE jir_finance_test;"

# Lancer l'application
cd src/JIR.WebAPI
dotnet run
```

## Vérifier les données générées

### Via l'API Swagger

1. Ouvrir https://localhost:5001/swagger
2. Tester les endpoints:
   - `GET /api/magasins` → Devrait retourner 17 magasins
   - `GET /api/articlesmagasin` → Devrait retourner ~100-150 articles
   - `GET /api/mouvementsstock` → Devrait retourner plusieurs mouvements

### Via PostgreSQL

```sql
-- Compter les enregistrements
SELECT 
    (SELECT COUNT(*) FROM magasins) as magasins,
    (SELECT COUNT(*) FROM articles_magasin) as articles,
    (SELECT COUNT(*) FROM mouvements_stock) as mouvements,
    (SELECT COUNT(*) FROM sections) as sections,
    (SELECT COUNT(*) FROM partenaires) as partenaires,
    (SELECT COUNT(*) FROM recettes) as recettes,
    (SELECT COUNT(*) FROM depenses) as depenses,
    (SELECT COUNT(*) FROM cotisations) as cotisations;
```

### Via l'application Blazor

1. Ouvrir https://localhost:5002
2. Naviguer vers la page Dashboard
3. Les graphiques devraient afficher les données seedées:
   - Graphique en camembert des mouvements
   - Graphique en ligne des mouvements des 7 derniers jours
   - Top 10 des articles avec stock bas

## Modifier les données de seed

Pour personnaliser les données générées, éditez:
```
src/JIR.Infrastructure/Data/DatabaseSeeder.cs
```

### Exemples de modifications

**Ajouter plus de magasins:**
```csharp
magasins.Add(new Magasin 
{ 
    Nom = "Mon Nouveau Magasin", 
    Adresse = "Adresse personnalisée", 
    Telephone = "514-XXX-XXXX" 
});
```

**Modifier les articles:**
```csharp
var nomsArticles = new[]
{
    "Mon Article 1", "Mon Article 2", // etc.
};
```

**Ajuster les quantités:**
```csharp
QuantiteStock = random.Next(50, 200), // Plus de stock
SeuilAlerte = random.Next(10, 30),    // Seuils plus élevés
```

## Désactiver le seeding

Pour désactiver temporairement le seeding:

### Option 1: Commenter le code dans Program.cs

```csharp
// await DatabaseSeeder.SeedAsync(context);
```

### Option 2: Modifier DatabaseSeeder.cs

```csharp
public static async Task SeedAsync(ApplicationDbContext context)
{
    return; // Toujours skipper le seeding
    
    // ... reste du code
}
```

## Production sur Fly.io

Le seeding s'exécutera automatiquement lors du premier déploiement:

1. L'application démarre
2. Les migrations créent les tables
3. Le seeder détecte une base vide
4. Les données de démonstration sont créées

**Note**: Pour la production réelle, vous voudrez peut-être désactiver le seeding ou utiliser des données réelles via un script de migration dédié.

## Troubleshooting

### "An error occurred while seeding"

Vérifiez:
- La connexion à la base de données fonctionne
- Les migrations sont appliquées
- Pas de contraintes de clés étrangères violées

### Les données ne s'affichent pas

1. Vérifiez les logs de l'application
2. Testez les endpoints API directement
3. Vérifiez que le seeding s'est bien exécuté:
   ```sql
   SELECT COUNT(*) FROM magasins;
   ```

### Dupliquer les données à chaque redémarrage

Le seeder vérifie si des données existent avant d'insérer:
```csharp
if (await context.Magasins.AnyAsync())
{
    return; // Skip si déjà des données
}
```

Si vous voyez des duplicatas, vérifiez que cette vérification fonctionne correctement.
