using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JIR.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMouvementStockConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "i_x_mouvements_stock_article_id",
                table: "mouvements_stock",
                newName: "ix_mouvements_stock_article_id");

            migrationBuilder.AlterColumn<string>(
                name: "type_mouvement",
                table: "mouvements_stock",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<decimal>(
                name: "quantite",
                table: "mouvements_stock",
                type: "numeric(15,3)",
                precision: 15,
                scale: 3,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<string>(
                name: "numero_document",
                table: "mouvements_stock",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "motif",
                table: "mouvements_stock",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "is_deleted",
                table: "mouvements_stock",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "created_by",
                table: "mouvements_stock",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "ix_mouvements_stock_date_mouvement",
                table: "mouvements_stock",
                column: "date_mouvement");

            migrationBuilder.CreateIndex(
                name: "ix_mouvements_stock_is_deleted",
                table: "mouvements_stock",
                column: "is_deleted");

            migrationBuilder.CreateIndex(
                name: "ix_mouvements_stock_type_mouvement",
                table: "mouvements_stock",
                column: "type_mouvement");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_mouvements_stock_date_mouvement",
                table: "mouvements_stock");

            migrationBuilder.DropIndex(
                name: "ix_mouvements_stock_is_deleted",
                table: "mouvements_stock");

            migrationBuilder.DropIndex(
                name: "ix_mouvements_stock_type_mouvement",
                table: "mouvements_stock");

            migrationBuilder.RenameIndex(
                name: "ix_mouvements_stock_article_id",
                table: "mouvements_stock",
                newName: "i_x_mouvements_stock_article_id");

            migrationBuilder.AlterColumn<int>(
                name: "type_mouvement",
                table: "mouvements_stock",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<decimal>(
                name: "quantite",
                table: "mouvements_stock",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(15,3)",
                oldPrecision: 15,
                oldScale: 3);

            migrationBuilder.AlterColumn<string>(
                name: "numero_document",
                table: "mouvements_stock",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "motif",
                table: "mouvements_stock",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "is_deleted",
                table: "mouvements_stock",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "created_by",
                table: "mouvements_stock",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);
        }
    }
}
