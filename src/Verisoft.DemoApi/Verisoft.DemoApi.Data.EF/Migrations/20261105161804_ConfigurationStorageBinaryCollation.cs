using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Verisoft.DemoApi.Data.EF.Migrations
{
    /// <inheritdoc />
    public partial class ConfigurationStorageBinaryCollation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_ComponentConfigurations_Owner",
                table: "ComponentConfigurations");

            migrationBuilder.DropIndex(
                name: "IX_ComponentConfigurations_Context_ComponentId_ConfigurationType",
                table: "ComponentConfigurations");

            migrationBuilder.DropIndex(
                name: "IX_ComponentConfigurations_Context_ComponentId_ConfigurationType_UserId",
                table: "ComponentConfigurations");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ComponentConfigurations",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                collation: "Latin1_General_100_BIN2",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Context",
                table: "ComponentConfigurations",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                collation: "Latin1_General_100_BIN2",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "ConfigurationType",
                table: "ComponentConfigurations",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                collation: "Latin1_General_100_BIN2",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "ComponentId",
                table: "ComponentConfigurations",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                collation: "Latin1_General_100_BIN2",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

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

            migrationBuilder.AddCheckConstraint(
                name: "CK_ComponentConfigurations_Owner",
                table: "ComponentConfigurations",
                sql: "([Level] = 1 AND [UserId] IS NULL) OR ([Level] = 2 AND [UserId] IS NOT NULL)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_ComponentConfigurations_Owner",
                table: "ComponentConfigurations");

            migrationBuilder.DropIndex(
                name: "IX_ComponentConfigurations_Context_ComponentId_ConfigurationType",
                table: "ComponentConfigurations");

            migrationBuilder.DropIndex(
                name: "IX_ComponentConfigurations_Context_ComponentId_ConfigurationType_UserId",
                table: "ComponentConfigurations");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ComponentConfigurations",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true,
                oldCollation: "Latin1_General_100_BIN2");

            migrationBuilder.AlterColumn<string>(
                name: "Context",
                table: "ComponentConfigurations",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldCollation: "Latin1_General_100_BIN2");

            migrationBuilder.AlterColumn<string>(
                name: "ConfigurationType",
                table: "ComponentConfigurations",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldCollation: "Latin1_General_100_BIN2");

            migrationBuilder.AlterColumn<string>(
                name: "ComponentId",
                table: "ComponentConfigurations",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldCollation: "Latin1_General_100_BIN2");

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

            migrationBuilder.AddCheckConstraint(
                name: "CK_ComponentConfigurations_Owner",
                table: "ComponentConfigurations",
                sql: "([Level] = 1 AND [UserId] IS NULL) OR ([Level] = 2 AND [UserId] IS NOT NULL)");
        }
    }
}
