using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseFlow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LinkPersonnelToIdentityUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Personnels_PersonnelId1",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentTransactions_Expenses_ExpenseId1",
                table: "PaymentTransactions");

            migrationBuilder.DropIndex(
                name: "IX_PaymentTransactions_ExpenseId1",
                table: "PaymentTransactions");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_PersonnelId1",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "ExpenseId1",
                table: "PaymentTransactions");

            migrationBuilder.DropColumn(
                name: "PersonnelId1",
                table: "Expenses");

            migrationBuilder.CreateIndex(
                name: "IX_AccountInfos_IBAN",
                table: "AccountInfos",
                column: "IBAN",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AccountInfos_IBAN",
                table: "AccountInfos");

            migrationBuilder.AddColumn<Guid>(
                name: "ExpenseId1",
                table: "PaymentTransactions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "PersonnelId1",
                table: "Expenses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransactions_ExpenseId1",
                table: "PaymentTransactions",
                column: "ExpenseId1");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_PersonnelId1",
                table: "Expenses",
                column: "PersonnelId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Personnels_PersonnelId1",
                table: "Expenses",
                column: "PersonnelId1",
                principalTable: "Personnels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentTransactions_Expenses_ExpenseId1",
                table: "PaymentTransactions",
                column: "ExpenseId1",
                principalTable: "Expenses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
