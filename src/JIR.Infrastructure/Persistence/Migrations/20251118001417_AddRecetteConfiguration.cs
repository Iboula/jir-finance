using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JIR.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddRecetteConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "i_x_recettes_section_id",
                table: "recettes",
                newName: "ix_recettes_section_id");

            migrationBuilder.AlterColumn<string>(
                name: "source",
                table: "recettes",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "recu_numero",
                table: "recettes",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "piece_justificative",
                table: "recettes",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "observations",
                table: "recettes",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "numero_document",
                table: "recettes",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "montant",
                table: "recettes",
                type: "numeric(15,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<string>(
                name: "mode_encaissement",
                table: "recettes",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "libelle",
                table: "recettes",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<bool>(
                name: "is_deleted",
                table: "recettes",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "created_by",
                table: "recettes",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "categorie",
                table: "recettes",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "ix_recettes_categorie",
                table: "recettes",
                column: "categorie");

            migrationBuilder.CreateIndex(
                name: "ix_recettes_date_recette",
                table: "recettes",
                column: "date_recette");

            migrationBuilder.CreateIndex(
                name: "ix_recettes_is_deleted",
                table: "recettes",
                column: "is_deleted");

            migrationBuilder.CreateIndex(
                name: "ix_recettes_section_date",
                table: "recettes",
                columns: new[] { "section_id", "date_recette" });

            migrationBuilder.CreateIndex(
                name: "ix_recettes_source",
                table: "recettes",
                column: "source");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_recettes_categorie",
                table: "recettes");

            migrationBuilder.DropIndex(
                name: "ix_recettes_date_recette",
                table: "recettes");

            migrationBuilder.DropIndex(
                name: "ix_recettes_is_deleted",
                table: "recettes");

            migrationBuilder.DropIndex(
                name: "ix_recettes_section_date",
                table: "recettes");

            migrationBuilder.DropIndex(
                name: "ix_recettes_source",
                table: "recettes");

            migrationBuilder.RenameIndex(
                name: "ix_recettes_section_id",
                table: "recettes",
                newName: "i_x_recettes_section_id");

            migrationBuilder.AlterColumn<string>(
                name: "source",
                table: "recettes",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "recu_numero",
                table: "recettes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "piece_justificative",
                table: "recettes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "observations",
                table: "recettes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "numero_document",
                table: "recettes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "montant",
                table: "recettes",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(15,2)");

            migrationBuilder.AlterColumn<string>(
                name: "mode_encaissement",
                table: "recettes",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "libelle",
                table: "recettes",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<bool>(
                name: "is_deleted",
                table: "recettes",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "created_by",
                table: "recettes",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "categorie",
                table: "recettes",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);
        }
    }
}
