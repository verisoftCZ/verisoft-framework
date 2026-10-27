using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Verisoft.DemoApi.Data.EF.Migrations
{
    /// <inheritdoc />
    public partial class UniqueFilterCompositionTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CodebookParent_ClientId",
                table: "CodebookParent");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "CodebookParent",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_CodebookParent_ClientId_Code",
                table: "CodebookParent",
                columns: new[] { "ClientId", "Code" },
                unique: true,
                filter: "(IsActive = 1) AND [IsDeleted] = CAST(0 AS bit)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CodebookParent_ClientId_Code",
                table: "CodebookParent");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "CodebookParent",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_CodebookParent_ClientId",
                table: "CodebookParent",
                column: "ClientId");
        }
    }
}
