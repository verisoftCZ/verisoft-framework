using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Verisoft.CodebookApi.Host.Migrations
{
    /// <inheritdoc />
    public partial class AddCodebook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "codebook");

            migrationBuilder.CreateTable(
                name: "Codebook",
                schema: "codebook",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsExternal = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Codebook", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Codebook.Accessibility",
                schema: "codebook",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StringValue = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ValidFrom = table.Column<DateOnly>(type: "date", nullable: false),
                    ValidTo = table.Column<DateOnly>(type: "date", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Codebook.Accessibility", x => x.Id);
                    table.UniqueConstraint("AK_Codebook.Accessibility_StringValue", x => x.StringValue);
                });

            migrationBuilder.CreateTable(
                name: "Language",
                schema: "codebook",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Language", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Codebook.AccessibilityTranslation",
                schema: "codebook",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CodebookRecordId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Codebook.AccessibilityTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Codebook.AccessibilityTranslation_Codebook.Accessibility_CodebookRecordId",
                        column: x => x.CodebookRecordId,
                        principalSchema: "codebook",
                        principalTable: "Codebook.Accessibility",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "codebook",
                table: "Language",
                columns: new[] { "Id", "Code", "CreatedAt", "CreatedBy", "IsDefault", "IsDeleted", "Name", "UpdatedAt", "UpdatedBy" },
                values: new object[] { 1, "Default", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", true, false, "Default", null, "" });

            migrationBuilder.CreateIndex(
                name: "IX_Codebook.AccessibilityTranslation_CodebookRecordId",
                schema: "codebook",
                table: "Codebook.AccessibilityTranslation",
                column: "CodebookRecordId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Codebook",
                schema: "codebook");

            migrationBuilder.DropTable(
                name: "Codebook.AccessibilityTranslation",
                schema: "codebook");

            migrationBuilder.DropTable(
                name: "Language",
                schema: "codebook");

            migrationBuilder.DropTable(
                name: "Codebook.Accessibility",
                schema: "codebook");
        }
    }
}
