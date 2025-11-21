using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JIR.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMagasinConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "f_k_articles_magasin__magasins_magasin_id",
                table: "articles_magasin");

            migrationBuilder.DropForeignKey(
                name: "f_k_magasins__users_responsable_id",
                table: "magasins");

            migrationBuilder.RenameIndex(
                name: "i_x_magasins_code",
                table: "magasins",
                newName: "ix_magasins_code");

            migrationBuilder.AlterColumn<string>(
                name: "localisation",
                table: "magasins",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "is_deleted",
                table: "magasins",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "created_by",
                table: "magasins",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "code",
                table: "magasins",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.CreateIndex(
                name: "ix_magasins_is_deleted",
                table: "magasins",
                column: "is_deleted");

            migrationBuilder.AddForeignKey(
                name: "f_k_articles_magasin_magasins_magasin_id",
                table: "articles_magasin",
                column: "magasin_id",
                principalTable: "magasins",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "f_k_magasins__users_responsable_id",
                table: "magasins",
                column: "responsable_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "f_k_articles_magasin_magasins_magasin_id",
                table: "articles_magasin");

            migrationBuilder.DropForeignKey(
                name: "f_k_magasins__users_responsable_id",
                table: "magasins");

            migrationBuilder.DropIndex(
                name: "ix_magasins_is_deleted",
                table: "magasins");

            migrationBuilder.RenameIndex(
                name: "ix_magasins_code",
                table: "magasins",
                newName: "i_x_magasins_code");

            migrationBuilder.AlterColumn<string>(
                name: "localisation",
                table: "magasins",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "is_deleted",
                table: "magasins",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "created_by",
                table: "magasins",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "code",
                table: "magasins",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AddForeignKey(
                name: "f_k_articles_magasin__magasins_magasin_id",
                table: "articles_magasin",
                column: "magasin_id",
                principalTable: "magasins",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "f_k_magasins__users_responsable_id",
                table: "magasins",
                column: "responsable_id",
                principalTable: "users",
                principalColumn: "id");
        }
    }
}
