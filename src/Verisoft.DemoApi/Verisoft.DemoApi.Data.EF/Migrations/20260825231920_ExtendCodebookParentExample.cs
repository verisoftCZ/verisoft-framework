using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Verisoft.DemoApi.Data.EF.Migrations
{
    /// <inheritdoc />
    public partial class ExtendCodebookParentExample : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "CodebookParent",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "CodebookParent",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "CodebookParent");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "CodebookParent");
        }
    }
}
