using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Verisoft.DemoApi.Data.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDeleteFilterToUniqueIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Blob_DocumentId",
                table: "Blob");

            migrationBuilder.CreateIndex(
                name: "IX_Blob_DocumentId",
                table: "Blob",
                column: "DocumentId",
                unique: true,
                filter: "[IsDeleted] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Blob_DocumentId",
                table: "Blob");

            migrationBuilder.CreateIndex(
                name: "IX_Blob_DocumentId",
                table: "Blob",
                column: "DocumentId",
                unique: true);
        }
    }
}
