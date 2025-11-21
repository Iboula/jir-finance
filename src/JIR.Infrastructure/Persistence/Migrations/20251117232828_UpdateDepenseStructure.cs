using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JIR.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDepenseStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "f_k_depenses__users_demandeur_id",
                table: "depenses");

            migrationBuilder.DropForeignKey(
                name: "f_k_depenses__users_validateur1_id",
                table: "depenses");

            migrationBuilder.DropForeignKey(
                name: "f_k_depenses__users_validateur2_id",
                table: "depenses");

            migrationBuilder.DropIndex(
                name: "i_x_depenses_demandeur_id",
                table: "depenses");

            migrationBuilder.DropColumn(
                name: "date_validation1",
                table: "depenses");

            migrationBuilder.DropColumn(
                name: "demandeur_id",
                table: "depenses");

            migrationBuilder.DropColumn(
                name: "numero_document",
                table: "depenses");

            migrationBuilder.DropColumn(
                name: "piece_justificative",
                table: "depenses");

            migrationBuilder.DropColumn(
                name: "statut_workflow",
                table: "depenses");

            migrationBuilder.RenameColumn(
                name: "validateur2_id",
                table: "depenses",
                newName: "user_id2");

            migrationBuilder.RenameColumn(
                name: "validateur1_id",
                table: "depenses",
                newName: "user_id1");

            migrationBuilder.RenameColumn(
                name: "libelle",
                table: "depenses",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "date_validation2",
                table: "depenses",
                newName: "date_validation");

            migrationBuilder.RenameIndex(
                name: "i_x_depenses_validateur2_id",
                table: "depenses",
                newName: "i_x_depenses_user_id2");

            migrationBuilder.RenameIndex(
                name: "i_x_depenses_validateur1_id",
                table: "depenses",
                newName: "i_x_depenses_user_id1");

            migrationBuilder.RenameIndex(
                name: "i_x_depenses_section_id",
                table: "depenses",
                newName: "ix_depenses_section_id");

            migrationBuilder.AlterColumn<string>(
                name: "observations",
                table: "depenses",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "mode_paiement",
                table: "depenses",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "is_deleted",
                table: "depenses",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "created_by",
                table: "depenses",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "motif_rejet",
                table: "depenses",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "numero",
                table: "depenses",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "reference_facture",
                table: "depenses",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "statut",
                table: "depenses",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "user_id",
                table: "depenses",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "valide_par",
                table: "depenses",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "i_x_depenses_user_id",
                table: "depenses",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_depenses_date_depense",
                table: "depenses",
                column: "date_depense");

            migrationBuilder.CreateIndex(
                name: "ix_depenses_is_deleted",
                table: "depenses",
                column: "is_deleted");

            migrationBuilder.CreateIndex(
                name: "ix_depenses_numero",
                table: "depenses",
                column: "numero",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_depenses_statut",
                table: "depenses",
                column: "statut");

            migrationBuilder.AddForeignKey(
                name: "f_k_depenses__users_user_id",
                table: "depenses",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "f_k_depenses__users_user_id1",
                table: "depenses",
                column: "user_id1",
                principalTable: "users",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "f_k_depenses__users_user_id2",
                table: "depenses",
                column: "user_id2",
                principalTable: "users",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "f_k_depenses__users_user_id",
                table: "depenses");

            migrationBuilder.DropForeignKey(
                name: "f_k_depenses__users_user_id1",
                table: "depenses");

            migrationBuilder.DropForeignKey(
                name: "f_k_depenses__users_user_id2",
                table: "depenses");

            migrationBuilder.DropIndex(
                name: "i_x_depenses_user_id",
                table: "depenses");

            migrationBuilder.DropIndex(
                name: "ix_depenses_date_depense",
                table: "depenses");

            migrationBuilder.DropIndex(
                name: "ix_depenses_is_deleted",
                table: "depenses");

            migrationBuilder.DropIndex(
                name: "ix_depenses_numero",
                table: "depenses");

            migrationBuilder.DropIndex(
                name: "ix_depenses_statut",
                table: "depenses");

            migrationBuilder.DropColumn(
                name: "motif_rejet",
                table: "depenses");

            migrationBuilder.DropColumn(
                name: "numero",
                table: "depenses");

            migrationBuilder.DropColumn(
                name: "reference_facture",
                table: "depenses");

            migrationBuilder.DropColumn(
                name: "statut",
                table: "depenses");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "depenses");

            migrationBuilder.DropColumn(
                name: "valide_par",
                table: "depenses");

            migrationBuilder.RenameColumn(
                name: "user_id2",
                table: "depenses",
                newName: "validateur2_id");

            migrationBuilder.RenameColumn(
                name: "user_id1",
                table: "depenses",
                newName: "validateur1_id");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "depenses",
                newName: "libelle");

            migrationBuilder.RenameColumn(
                name: "date_validation",
                table: "depenses",
                newName: "date_validation2");

            migrationBuilder.RenameIndex(
                name: "ix_depenses_section_id",
                table: "depenses",
                newName: "i_x_depenses_section_id");

            migrationBuilder.RenameIndex(
                name: "i_x_depenses_user_id2",
                table: "depenses",
                newName: "i_x_depenses_validateur2_id");

            migrationBuilder.RenameIndex(
                name: "i_x_depenses_user_id1",
                table: "depenses",
                newName: "i_x_depenses_validateur1_id");

            migrationBuilder.AlterColumn<string>(
                name: "observations",
                table: "depenses",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "mode_paiement",
                table: "depenses",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "is_deleted",
                table: "depenses",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "created_by",
                table: "depenses",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<DateTime>(
                name: "date_validation1",
                table: "depenses",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "demandeur_id",
                table: "depenses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "numero_document",
                table: "depenses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "piece_justificative",
                table: "depenses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "statut_workflow",
                table: "depenses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "i_x_depenses_demandeur_id",
                table: "depenses",
                column: "demandeur_id");

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
        }
    }
}
