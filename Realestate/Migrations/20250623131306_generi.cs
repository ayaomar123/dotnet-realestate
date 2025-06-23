using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Realestate.Migrations
{
    /// <inheritdoc />
    public partial class generi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MyTypes",
                table: "MyTypes");

            migrationBuilder.RenameTable(
                name: "MyTypes",
                newName: "Types");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Types",
                table: "Types",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Types",
                table: "Types");

            migrationBuilder.RenameTable(
                name: "Types",
                newName: "MyTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MyTypes",
                table: "MyTypes",
                column: "Id");
        }
    }
}
