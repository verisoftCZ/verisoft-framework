using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Verisoft.DemoApi.Data.EF.Migrations
{
    /// <inheritdoc />
    public partial class ConfigurationStorageFrameworkEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ComponentConfigurations_Context_ComponentId_ConfigurationType",
                table: "ComponentConfigurations");

            migrationBuilder.DropIndex(
                name: "IX_ComponentConfigurations_Context_ComponentId_ConfigurationType_UserId",
                table: "ComponentConfigurations");

            migrationBuilder.Sql("UPDATE [ComponentConfigurations] SET [UpdatedAt] = SWITCHOFFSET([UpdatedAt], '+00:00');");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "ComponentConfigurations",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ComponentConfigurations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ComponentConfigurations",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ComponentConfigurations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "ComponentConfigurations",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.Sql("UPDATE [ComponentConfigurations] SET [CreatedAt] = [UpdatedAt], [CreatedBy] = N'configuration-storage:migration';");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentConfigurations_Context_ComponentId_ConfigurationType",
                table: "ComponentConfigurations",
                columns: new[] { "Context", "ComponentId", "ConfigurationType" },
                unique: true,
                filter: "([Level] = 1) AND [IsDeleted] = CAST(0 AS bit)");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentConfigurations_Context_ComponentId_ConfigurationType_UserId",
                table: "ComponentConfigurations",
                columns: new[] { "Context", "ComponentId", "ConfigurationType", "UserId" },
                unique: true,
                filter: "([Level] = 2) AND [IsDeleted] = CAST(0 AS bit)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [ComponentConfigurations] WHERE [IsDeleted] = 1;");
            migrationBuilder.Sql("UPDATE [ComponentConfigurations] SET [UpdatedAt] = COALESCE([UpdatedAt], [CreatedAt]) WHERE [UpdatedAt] IS NULL;");

            migrationBuilder.DropIndex(
                name: "IX_ComponentConfigurations_Context_ComponentId_ConfigurationType",
                table: "ComponentConfigurations");

            migrationBuilder.DropIndex(
                name: "IX_ComponentConfigurations_Context_ComponentId_ConfigurationType_UserId",
                table: "ComponentConfigurations");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ComponentConfigurations");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ComponentConfigurations");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ComponentConfigurations");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "ComponentConfigurations");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "ComponentConfigurations",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

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
    }
}
