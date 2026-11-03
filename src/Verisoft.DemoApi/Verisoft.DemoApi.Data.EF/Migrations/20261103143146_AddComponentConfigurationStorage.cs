using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Verisoft.DemoApi.Data.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddComponentConfigurationStorage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ComponentConfigurations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Context = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ComponentId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ConfigurationType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SchemaVersion = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentConfigurations", x => x.Id);
                    table.CheckConstraint("CK_ComponentConfigurations_Owner", "([Level] = 1 AND [UserId] IS NULL) OR ([Level] = 2 AND [UserId] IS NOT NULL)");
                    table.CheckConstraint("CK_ComponentConfigurations_SchemaVersion", "[SchemaVersion] > 0");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComponentConfigurations_Context_ComponentId_ConfigurationType",
                table: "ComponentConfigurations",
                columns: new[] { "Context", "ComponentId", "ConfigurationType" },
                unique: true,
                filter: "[Level] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentConfigurations_Context_ComponentId_ConfigurationType_UserId",
                table: "ComponentConfigurations",
                columns: new[] { "Context", "ComponentId", "ConfigurationType", "UserId" },
                unique: true,
                filter: "[Level] = 2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComponentConfigurations");
        }
    }
}
