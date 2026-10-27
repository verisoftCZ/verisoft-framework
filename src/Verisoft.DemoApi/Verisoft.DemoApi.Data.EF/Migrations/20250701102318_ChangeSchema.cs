using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Verisoft.DemoApi.Data.EF.Migrations
{
    /// <inheritdoc />
    public partial class ChangeSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DocumentSagaStates",
                table: "DocumentSagaState");

            migrationBuilder.EnsureSchema(
                name: "MassTransit");

            migrationBuilder.RenameTable(
                name: "DocumentSagaState",
                newName: "DocumentSagaState",
                newSchema: "MassTransit");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DocumentSagaState",
                schema: "MassTransit",
                table: "DocumentSagaState",
                column: "CorrelationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DocumentSagaState",
                schema: "MassTransit",
                table: "DocumentSagaState");

            migrationBuilder.RenameTable(
                name: "DocumentSagaState",
                schema: "MassTransit",
                newName: "DocumentSagaState");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DocumentSagaStates",
                table: "DocumentSagaState",
                column: "CorrelationId");
        }
    }
}
