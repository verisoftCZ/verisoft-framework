using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Verisoft.DemoApi.Database.Migrations
{
    /// <inheritdoc />
    public partial class FixClientNaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Document_Client_ClientEntityId",
                table: "Document");

            migrationBuilder.DropIndex(
                name: "IX_Document_ClientEntityId",
                table: "Document");

            migrationBuilder.DropColumn(
                name: "ClientEntityId",
                table: "Document");

            migrationBuilder.CreateIndex(
                name: "IX_Document_ClientId",
                table: "Document",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Document_Client_ClientId",
                table: "Document",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Document_Client_ClientId",
                table: "Document");

            migrationBuilder.DropIndex(
                name: "IX_Document_ClientId",
                table: "Document");

            migrationBuilder.AddColumn<int>(
                name: "ClientEntityId",
                table: "Document",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Document_ClientEntityId",
                table: "Document",
                column: "ClientEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Document_Client_ClientEntityId",
                table: "Document",
                column: "ClientEntityId",
                principalTable: "Client",
                principalColumn: "Id");
        }
    }
}
