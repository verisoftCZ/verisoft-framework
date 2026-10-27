using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Verisoft.DemoApi.Data.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddClientToCodebookParent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "CodebookParent",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CodebookParent_ClientId",
                table: "CodebookParent",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_CodebookParent_Client_ClientId",
                table: "CodebookParent",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CodebookParent_Client_ClientId",
                table: "CodebookParent");

            migrationBuilder.DropIndex(
                name: "IX_CodebookParent_ClientId",
                table: "CodebookParent");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "CodebookParent");
        }
    }
}
