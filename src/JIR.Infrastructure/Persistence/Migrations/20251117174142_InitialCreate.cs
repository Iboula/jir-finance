using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JIR.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "partenaires",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false),
                    nom = table.Column<string>(type: "text", nullable: false),
                    type_partenaire = table.Column<int>(type: "integer", nullable: false),
                    adresse = table.Column<string>(type: "text", nullable: true),
                    telephone = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    contact_nom = table.Column<string>(type: "text", nullable: true),
                    observations = table.Column<string>(type: "text", nullable: true),
                    is_actif = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_partenaires", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "articles_magasin",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    magasin_id = table.Column<Guid>(type: "uuid", nullable: false),
                    code_article = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    designation = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    unite = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    quantite_stock = table.Column<decimal>(type: "numeric(15,3)", precision: 15, scale: 3, nullable: false),
                    seuil_alerte = table.Column<decimal>(type: "numeric(15,3)", precision: 15, scale: 3, nullable: false),
                    prix_unitaire = table.Column<decimal>(type: "numeric(15,2)", precision: 15, scale: 2, nullable: false),
                    observations = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_articles_magasin", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "mouvements_stock",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    article_id = table.Column<Guid>(type: "uuid", nullable: false),
                    type_mouvement = table.Column<int>(type: "integer", nullable: false),
                    quantite = table.Column<decimal>(type: "numeric", nullable: false),
                    date_mouvement = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    numero_document = table.Column<string>(type: "text", nullable: true),
                    motif = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mouvements_stock", x => x.id);
                    table.ForeignKey(
                        name: "f_k_mouvements_stock_articles_magasin_article_id",
                        column: x => x.article_id,
                        principalTable: "articles_magasin",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "cotisations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    section_id = table.Column<Guid>(type: "uuid", nullable: false),
                    membre_nom = table.Column<string>(type: "text", nullable: false),
                    membre_matricule = table.Column<string>(type: "text", nullable: false),
                    montant = table.Column<decimal>(type: "numeric", nullable: false),
                    periode = table.Column<string>(type: "text", nullable: false),
                    date_paiement = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    mode_paiement = table.Column<string>(type: "text", nullable: false),
                    recu_numero = table.Column<string>(type: "text", nullable: true),
                    statut = table.Column<string>(type: "text", nullable: false),
                    observations = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cotisations", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "depenses",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    section_id = table.Column<Guid>(type: "uuid", nullable: false),
                    libelle = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    montant = table.Column<decimal>(type: "numeric(15,2)", precision: 15, scale: 2, nullable: false),
                    date_depense = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    beneficiaire = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    categorie = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    mode_paiement = table.Column<string>(type: "text", nullable: true),
                    numero_document = table.Column<string>(type: "text", nullable: true),
                    piece_justificative = table.Column<string>(type: "text", nullable: true),
                    observations = table.Column<string>(type: "text", nullable: true),
                    statut_workflow = table.Column<int>(type: "integer", nullable: false),
                    demandeur_id = table.Column<Guid>(type: "uuid", nullable: false),
                    validateur1_id = table.Column<Guid>(type: "uuid", nullable: true),
                    validateur2_id = table.Column<Guid>(type: "uuid", nullable: true),
                    date_validation1 = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    date_validation2 = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    date_paiement = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_depenses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "magasins",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    nom = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    localisation = table.Column<string>(type: "text", nullable: true),
                    responsable_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_magasins", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "recettes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    section_id = table.Column<Guid>(type: "uuid", nullable: false),
                    libelle = table.Column<string>(type: "text", nullable: false),
                    montant = table.Column<decimal>(type: "numeric", nullable: false),
                    date_recette = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    source = table.Column<string>(type: "text", nullable: false),
                    categorie = table.Column<string>(type: "text", nullable: false),
                    recu_numero = table.Column<string>(type: "text", nullable: true),
                    mode_encaissement = table.Column<string>(type: "text", nullable: false),
                    numero_document = table.Column<string>(type: "text", nullable: true),
                    piece_justificative = table.Column<string>(type: "text", nullable: true),
                    observations = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recettes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sections",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    nom = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    responsable_id = table.Column<Guid>(type: "uuid", nullable: true),
                    budget_annuel = table.Column<decimal>(type: "numeric(15,2)", precision: 15, scale: 2, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sections", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    password_hash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    prenom = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    nom = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    role = table.Column<int>(type: "integer", nullable: false),
                    section_id = table.Column<Guid>(type: "uuid", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                    table.ForeignKey(
                        name: "f_k_users_sections_section_id",
                        column: x => x.section_id,
                        principalTable: "sections",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "i_x_articles_magasin_magasin_id_code_article",
                table: "articles_magasin",
                columns: new[] { "magasin_id", "code_article" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_cotisations_section_id",
                table: "cotisations",
                column: "section_id");

            migrationBuilder.CreateIndex(
                name: "i_x_depenses_demandeur_id",
                table: "depenses",
                column: "demandeur_id");

            migrationBuilder.CreateIndex(
                name: "i_x_depenses_section_id",
                table: "depenses",
                column: "section_id");

            migrationBuilder.CreateIndex(
                name: "i_x_depenses_validateur1_id",
                table: "depenses",
                column: "validateur1_id");

            migrationBuilder.CreateIndex(
                name: "i_x_depenses_validateur2_id",
                table: "depenses",
                column: "validateur2_id");

            migrationBuilder.CreateIndex(
                name: "i_x_magasins_code",
                table: "magasins",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_magasins_responsable_id",
                table: "magasins",
                column: "responsable_id");

            migrationBuilder.CreateIndex(
                name: "i_x_mouvements_stock_article_id",
                table: "mouvements_stock",
                column: "article_id");

            migrationBuilder.CreateIndex(
                name: "i_x_recettes_section_id",
                table: "recettes",
                column: "section_id");

            migrationBuilder.CreateIndex(
                name: "i_x_sections_code",
                table: "sections",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_sections_responsable_id",
                table: "sections",
                column: "responsable_id");

            migrationBuilder.CreateIndex(
                name: "i_x_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_users_section_id",
                table: "users",
                column: "section_id");

            migrationBuilder.CreateIndex(
                name: "i_x_users_username",
                table: "users",
                column: "username",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "f_k_articles_magasin__magasins_magasin_id",
                table: "articles_magasin",
                column: "magasin_id",
                principalTable: "magasins",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "f_k_cotisations__sections_section_id",
                table: "cotisations",
                column: "section_id",
                principalTable: "sections",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "f_k_depenses__sections_section_id",
                table: "depenses",
                column: "section_id",
                principalTable: "sections",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "f_k_depenses__users_demandeur_id",
                table: "depenses",
                column: "demandeur_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "f_k_depenses__users_validateur1_id",
                table: "depenses",
                column: "validateur1_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "f_k_depenses__users_validateur2_id",
                table: "depenses",
                column: "validateur2_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "f_k_magasins__users_responsable_id",
                table: "magasins",
                column: "responsable_id",
                principalTable: "users",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "f_k_recettes__sections_section_id",
                table: "recettes",
                column: "section_id",
                principalTable: "sections",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "f_k_sections__users_responsable_id",
                table: "sections",
                column: "responsable_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "f_k_users_sections_section_id",
                table: "users");

            migrationBuilder.DropTable(
                name: "cotisations");

            migrationBuilder.DropTable(
                name: "depenses");

            migrationBuilder.DropTable(
                name: "mouvements_stock");

            migrationBuilder.DropTable(
                name: "partenaires");

            migrationBuilder.DropTable(
                name: "recettes");

            migrationBuilder.DropTable(
                name: "articles_magasin");

            migrationBuilder.DropTable(
                name: "magasins");

            migrationBuilder.DropTable(
                name: "sections");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
