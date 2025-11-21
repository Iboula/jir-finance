using JIR.Domain.Entities;
using JIR.Domain.Enums;
using JIR.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace JIR.Infrastructure.Data;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        // Vérifier si la base de données a déjà des données
        if (await context.Magasins.AnyAsync())
        {
            return; // La base de données est déjà initialisée
        }

        var random = new Random();

        // 1. Créer des sections
        var sections = new List<Section>
        {
            new Section { Nom = "Direction", Description = "Direction générale et administration" },
            new Section { Nom = "Comptabilité", Description = "Gestion financière et comptable" },
            new Section { Nom = "Ressources Humaines", Description = "Gestion du personnel" },
            new Section { Nom = "Ventes", Description = "Équipe commerciale" },
            new Section { Nom = "Marketing", Description = "Promotion et communication" },
            new Section { Nom = "Logistique", Description = "Gestion des stocks et livraisons" },
            new Section { Nom = "IT", Description = "Support informatique" },
            new Section { Nom = "Service Client", Description = "Support et satisfaction client" }
        };
        await context.Sections.AddRangeAsync(sections);
        await context.SaveChangesAsync();

        // 2. Créer des magasins
        var magasins = new List<Magasin>
        {
            new Magasin { Code = "M001", Nom = "Magasin Central", Localisation = "123 Rue Principale, Montréal" },
            new Magasin { Code = "M002", Nom = "Succursale Nord", Localisation = "456 Avenue Nord, Laval" },
            new Magasin { Code = "M003", Nom = "Succursale Sud", Localisation = "789 Boulevard Sud, Longueuil" },
            new Magasin { Code = "M004", Nom = "Magasin Est", Localisation = "321 Rue Est, Anjou" },
            new Magasin { Code = "M005", Nom = "Magasin Ouest", Localisation = "654 Avenue Ouest, Dorval" },
            new Magasin { Code = "M006", Nom = "Centre Commercial", Localisation = "987 Place Commerce, Brossard" },
            new Magasin { Code = "M007", Nom = "Boutique Downtown", Localisation = "147 Rue Sainte-Catherine, Montréal" },
            new Magasin { Code = "M008", Nom = "Magasin Plateau", Localisation = "258 Avenue Mont-Royal, Montréal" },
            new Magasin { Code = "M009", Nom = "Succursale Verdun", Localisation = "369 Rue Wellington, Verdun" },
            new Magasin { Code = "M010", Nom = "Magasin Westmount", Localisation = "741 Avenue Greene, Westmount" },
            new Magasin { Code = "M011", Nom = "Boutique Outremont", Localisation = "852 Avenue Laurier, Outremont" },
            new Magasin { Code = "M012", Nom = "Magasin Rosemont", Localisation = "963 Rue Beaubien, Rosemont" },
            new Magasin { Code = "M013", Nom = "Succursale Hochelaga", Localisation = "159 Rue Ontario, Hochelaga" },
            new Magasin { Code = "M014", Nom = "Magasin Villeray", Localisation = "357 Rue Jarry, Villeray" },
            new Magasin { Code = "M015", Nom = "Boutique Ahuntsic", Localisation = "486 Rue Fleury, Ahuntsic" },
            new Magasin { Code = "M016", Nom = "Magasin LaSalle", Localisation = "597 Boulevard LaSalle, LaSalle" },
            new Magasin { Code = "M017", Nom = "Succursale Lachine", Localisation = "608 Rue Notre-Dame, Lachine" }
        };
        await context.Magasins.AddRangeAsync(magasins);
        await context.SaveChangesAsync();

        // 3. Créer des articles pour chaque magasin
        var articles = new List<ArticleMagasin>();
        var nomsArticles = new[]
        {
            "Ordinateur portable", "Souris sans fil", "Clavier mécanique", "Écran 24 pouces",
            "Casque audio", "Webcam HD", "Imprimante laser", "Scanner portable",
            "Disque dur externe 1TB", "Clé USB 64GB", "Câble HDMI", "Adaptateur USB-C",
            "Tablette graphique", "Microphone USB", "Haut-parleurs Bluetooth", "Routeur WiFi",
            "Switch réseau", "Caméra de sécurité", "Lampe LED bureau", "Support ordinateur portable",
            "Tapis de souris gaming"
        };

        int codeArticleCounter = 1;
        foreach (var magasin in magasins)
        {
            // Créer 5 à 10 articles par magasin
            int nombreArticles = random.Next(5, 11);
            var articlesSelectionnes = nomsArticles.OrderBy(x => random.Next()).Take(nombreArticles);

            foreach (var nomArticle in articlesSelectionnes)
            {
                articles.Add(new ArticleMagasin
                {
                    CodeArticle = $"ART{codeArticleCounter:D4}",
                    Designation = nomArticle,
                    Unite = "Unité",
                    PrixUnitaire = random.Next(10, 1000),
                    QuantiteStock = random.Next(0, 100),
                    SeuilAlerte = random.Next(5, 20),
                    MagasinId = magasin.Id
                });
                codeArticleCounter++;
            }
        }
        await context.ArticlesMagasin.AddRangeAsync(articles);
        await context.SaveChangesAsync();

        // 4. Créer des mouvements de stock
        var mouvements = new List<MouvementStock>();
        var typesMotifs = new Dictionary<TypeMouvement, string[]>
        {
            { TypeMouvement.Entree, new[] { "Réapprovisionnement", "Livraison fournisseur", "Transfert entrant", "Retour client" } },
            { TypeMouvement.Sortie, new[] { "Vente client", "Transfert sortant", "Inventaire endommagé", "Échantillon" } },
            { TypeMouvement.Transfert, new[] { "Transfert inter-magasins", "Réorganisation stock", "Redistribution" } }
        };

        var articlesAvecMouvements = articles.OrderBy(x => random.Next()).Take(articles.Count / 2);
        foreach (var article in articlesAvecMouvements)
        {
            int nombreMouvements = random.Next(2, 6);
            for (int i = 0; i < nombreMouvements; i++)
            {
                var typeMouvement = (TypeMouvement)random.Next(0, 3);
                var motifs = typesMotifs[typeMouvement];
                
                mouvements.Add(new MouvementStock
                {
                    ArticleId = article.Id,
                    TypeMouvement = typeMouvement,
                    Quantite = random.Next(1, 20),
                    DateMouvement = DateTime.UtcNow.AddDays(-random.Next(0, 90)),
                    NumeroDocument = $"DOC{random.Next(1000, 9999)}",
                    Motif = motifs[random.Next(motifs.Length)]
                });
            }
        }
        await context.MouvementsStock.AddRangeAsync(mouvements);
        await context.SaveChangesAsync();

        // 5. Créer des partenaires
        var partenaires = new List<Partenaire>
        {
            new Partenaire { Code = "P001", Nom = "Fournisseur Tech Plus", TypePartenaire = TypePartenaire.Fournisseur, Email = "contact@techplus.com", Telephone = "514-111-2222" },
            new Partenaire { Code = "P002", Nom = "Distributeur Électro", TypePartenaire = TypePartenaire.Fournisseur, Email = "info@electro.com", Telephone = "514-222-3333" },
            new Partenaire { Code = "P003", Nom = "Client Entreprise ABC", TypePartenaire = TypePartenaire.Client, Email = "achat@abc.com", Telephone = "514-333-4444" },
            new Partenaire { Code = "P004", Nom = "Client Corporation XYZ", TypePartenaire = TypePartenaire.Client, Email = "commandes@xyz.com", Telephone = "514-444-5555" },
            new Partenaire { Code = "P005", Nom = "Transporteur Express", TypePartenaire = TypePartenaire.Autre, Email = "service@express.com", Telephone = "514-555-6666" },
            new Partenaire { Code = "P006", Nom = "Consultant Gestion", TypePartenaire = TypePartenaire.Autre, Email = "consultation@gestion.com", Telephone = "514-666-7777" }
        };
        await context.Partenaires.AddRangeAsync(partenaires);
        await context.SaveChangesAsync();

        // 6. Créer des recettes
        var recettes = new List<Recette>();
        var sources = new[] { "Vente", "Prestation", "Cotisation", "Don", "Subvention" };
        var categories = new[] { "Produits", "Services", "Formation", "Conseil", "Autre" };
        var modesEncaissement = new[] { "Espèces", "Chèque", "Virement", "Carte bancaire" };

        for (int i = 1; i <= 10; i++)
        {
            recettes.Add(new Recette
            {
                SectionId = sections[random.Next(sections.Count)].Id,
                Libelle = $"Recette {i} - {sources[random.Next(sources.Length)]}",
                Montant = random.Next(1000, 50000),
                DateRecette = DateTime.UtcNow.AddDays(-random.Next(0, 90)),
                Source = sources[random.Next(sources.Length)],
                Categorie = categories[random.Next(categories.Length)],
                RecuNumero = $"R{DateTime.UtcNow.Year}-{random.Next(1000, 9999)}",
                ModeEncaissement = modesEncaissement[random.Next(modesEncaissement.Length)],
                NumeroDocument = $"DOC{random.Next(10000, 99999)}"
            });
        }
        await context.Recettes.AddRangeAsync(recettes);
        await context.SaveChangesAsync();

        // 7. Créer des dépenses
        var categoriesDepenses = new[] { "Fournitures", "Salaires", "Loyer", "Électricité", "Internet", "Assurances", "Marketing", "Formation", "Maintenance", "Transport" };
        var statutsDepense = new[] { "Brouillon", "EnAttenteValidation", "Validé", "Payé" };
        var modesPaiement = new[] { "Espèces", "Chèque", "Virement", "Carte bancaire" };
        var depenses = new List<Depense>();
        
        for (int i = 1; i <= 15; i++)
        {
            depenses.Add(new Depense
            {
                SectionId = sections[random.Next(sections.Count)].Id,
                Numero = $"DEP{DateTime.UtcNow.Year}-{i:D4}",
                DateDepense = DateTime.UtcNow.AddDays(-random.Next(0, 90)),
                Categorie = categoriesDepenses[random.Next(categoriesDepenses.Length)],
                Description = $"Dépense - {categoriesDepenses[random.Next(categoriesDepenses.Length)]}",
                Montant = random.Next(500, 10000),
                Beneficiaire = $"Bénéficiaire {i}",
                ModePaiement = modesPaiement[random.Next(modesPaiement.Length)],
                ReferenceFacture = $"FACT-{random.Next(1000, 9999)}",
                Statut = statutsDepense[random.Next(statutsDepense.Length)]
            });
        }
        await context.Depenses.AddRangeAsync(depenses);
        await context.SaveChangesAsync();

        // 8. Créer des cotisations
        var cotisations = new List<Cotisation>();
        var statutsCotisation = new[] { "En attente", "Payé", "En retard" };
        var modesPaiementCotisation = new[] { "Espèces", "Chèque", "Virement", "Prélèvement" };

        for (int i = 1; i <= 10; i++)
        {
            var datePaiement = DateTime.UtcNow.AddDays(-random.Next(0, 30));
            cotisations.Add(new Cotisation
            {
                SectionId = sections[random.Next(sections.Count)].Id,
                MembreNom = $"Membre {i}",
                MembreMatricule = $"MAT{i:D4}",
                Montant = random.Next(100, 1000),
                Periode = datePaiement.ToString("yyyy-MM"),
                DatePaiement = datePaiement,
                ModePaiement = modesPaiementCotisation[random.Next(modesPaiementCotisation.Length)],
                RecuNumero = $"RC{datePaiement.Year}-{random.Next(1000, 9999)}",
                Statut = statutsCotisation[random.Next(statutsCotisation.Length)]
            });
        }
        await context.Cotisations.AddRangeAsync(cotisations);
        await context.SaveChangesAsync();
    }
}
