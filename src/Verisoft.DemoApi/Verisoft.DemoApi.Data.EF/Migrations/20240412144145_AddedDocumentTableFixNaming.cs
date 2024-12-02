using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Verisoft.DemoApi.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddedDocumentTableFixNaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentEntity_Client_ClientEntityId",
                table: "DocumentEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DocumentEntity",
                table: "DocumentEntity");

            migrationBuilder.RenameTable(
                name: "DocumentEntity",
                newName: "Document");

            migrationBuilder.RenameIndex(
                name: "IX_DocumentEntity_ClientEntityId",
                table: "Document",
                newName: "IX_Document_ClientEntityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Document",
                table: "Document",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Document_Client_ClientEntityId",
                table: "Document",
                column: "ClientEntityId",
                principalTable: "Client",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Document_Client_ClientEntityId",
                table: "Document");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Document",
                table: "Document");

            migrationBuilder.RenameTable(
                name: "Document",
                newName: "DocumentEntity");

            migrationBuilder.RenameIndex(
                name: "IX_Document_ClientEntityId",
                table: "DocumentEntity",
                newName: "IX_DocumentEntity_ClientEntityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DocumentEntity",
                table: "DocumentEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentEntity_Client_ClientEntityId",
                table: "DocumentEntity",
                column: "ClientEntityId",
                principalTable: "Client",
                principalColumn: "Id");
        }
    }
}
