using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Verisoft.DemoApi.Database.Migrations
{
    /// <inheritdoc />
    public partial class Add_AuditTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "audit");

            migrationBuilder.CreateTable(
                name: "EntityAudit",
                schema: "audit",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Schema = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Table = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ChangedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityAudit", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EntityAuditDetail",
                schema: "audit",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChangedAttribute = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    OldValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityAuditId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityAuditDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityAuditDetail_EntityAudit_EntityAuditId",
                        column: x => x.EntityAuditId,
                        principalSchema: "audit",
                        principalTable: "EntityAudit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EntityAudit_EntityId",
                schema: "audit",
                table: "EntityAudit",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityAudit_ChangedBy",
                schema: "audit",
                table: "EntityAudit",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EntityAudit_Table",
                schema: "audit",
                table: "EntityAudit",
                column: "Table");

            migrationBuilder.CreateIndex(
                name: "IX_EntityAuditDetail_EntityAuditId",
                schema: "audit",
                table: "EntityAuditDetail",
                column: "EntityAuditId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityAuditDetail_ChangedAttribute",
                schema: "audit",
                table: "EntityAuditDetail",
                column: "ChangedAttribute");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntityAuditDetail",
                schema: "audit");

            migrationBuilder.DropTable(
                name: "EntityAudit",
                schema: "audit");
        }
    }
}
