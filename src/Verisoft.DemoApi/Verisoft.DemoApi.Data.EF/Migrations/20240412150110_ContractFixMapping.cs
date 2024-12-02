using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Verisoft.DemoApi.Database.Migrations
{
    /// <inheritdoc />
    public partial class ContractFixMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contract_Client_ClientEntityId",
                table: "Contract");

            migrationBuilder.DropIndex(
                name: "IX_Contract_ClientEntityId",
                table: "Contract");

            migrationBuilder.DropColumn(
                name: "ClientEntityId",
                table: "Contract");

            migrationBuilder.CreateIndex(
                name: "IX_Contract_ClientId",
                table: "Contract",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contract_Client_ClientId",
                table: "Contract",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contract_Client_ClientId",
                table: "Contract");

            migrationBuilder.DropIndex(
                name: "IX_Contract_ClientId",
                table: "Contract");

            migrationBuilder.AddColumn<int>(
                name: "ClientEntityId",
                table: "Contract",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contract_ClientEntityId",
                table: "Contract",
                column: "ClientEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contract_Client_ClientEntityId",
                table: "Contract",
                column: "ClientEntityId",
                principalTable: "Client",
                principalColumn: "Id");
        }
    }
}
