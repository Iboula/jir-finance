using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JIR.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CleanupDepenseUserReferences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "i_x_depenses_user_id1",
                table: "depenses");

            migrationBuilder.DropIndex(
                name: "i_x_depenses_user_id2",
                table: "depenses");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "depenses");

            migrationBuilder.DropColumn(
                name: "user_id1",
                table: "depenses");

            migrationBuilder.DropColumn(
                name: "user_id2",
                table: "depenses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "user_id",
                table: "depenses",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "user_id1",
                table: "depenses",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "user_id2",
                table: "depenses",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "i_x_depenses_user_id",
                table: "depenses",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "i_x_depenses_user_id1",
                table: "depenses",
                column: "user_id1");

            migrationBuilder.CreateIndex(
                name: "i_x_depenses_user_id2",
                table: "depenses",
                column: "user_id2");

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
    }
}
