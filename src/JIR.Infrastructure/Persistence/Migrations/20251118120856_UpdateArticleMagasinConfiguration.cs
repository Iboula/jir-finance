using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JIR.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateArticleMagasinConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "i_x_articles_magasin_magasin_id_code_article",
                table: "articles_magasin",
                newName: "ix_articles_magasin_magasin_code");

            migrationBuilder.AlterColumn<decimal>(
                name: "seuil_alerte",
                table: "articles_magasin",
                type: "numeric(15,3)",
                precision: 15,
                scale: 3,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(15,3)",
                oldPrecision: 15,
                oldScale: 3);

            migrationBuilder.AlterColumn<decimal>(
                name: "quantite_stock",
                table: "articles_magasin",
                type: "numeric(15,3)",
                precision: 15,
                scale: 3,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(15,3)",
                oldPrecision: 15,
                oldScale: 3);

            migrationBuilder.AlterColumn<decimal>(
                name: "prix_unitaire",
                table: "articles_magasin",
                type: "numeric(15,2)",
                precision: 15,
                scale: 2,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(15,2)",
                oldPrecision: 15,
                oldScale: 2);

            migrationBuilder.AlterColumn<string>(
                name: "observations",
                table: "articles_magasin",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "is_deleted",
                table: "articles_magasin",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "created_by",
                table: "articles_magasin",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "ix_articles_magasin_is_deleted",
                table: "articles_magasin",
                column: "is_deleted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_articles_magasin_is_deleted",
                table: "articles_magasin");

            migrationBuilder.RenameIndex(
                name: "ix_articles_magasin_magasin_code",
                table: "articles_magasin",
                newName: "i_x_articles_magasin_magasin_id_code_article");

            migrationBuilder.AlterColumn<decimal>(
                name: "seuil_alerte",
                table: "articles_magasin",
                type: "numeric(15,3)",
                precision: 15,
                scale: 3,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(15,3)",
                oldPrecision: 15,
                oldScale: 3,
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "quantite_stock",
                table: "articles_magasin",
                type: "numeric(15,3)",
                precision: 15,
                scale: 3,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(15,3)",
                oldPrecision: 15,
                oldScale: 3,
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "prix_unitaire",
                table: "articles_magasin",
                type: "numeric(15,2)",
                precision: 15,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(15,2)",
                oldPrecision: 15,
                oldScale: 2,
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<string>(
                name: "observations",
                table: "articles_magasin",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "is_deleted",
                table: "articles_magasin",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "created_by",
                table: "articles_magasin",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);
        }
    }
}
