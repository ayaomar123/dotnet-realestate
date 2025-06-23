using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Realestate.Migrations
{
    /// <inheritdoc />
    public partial class item2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Districts_DisrtictId",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "DisrtictId",
                table: "Items",
                newName: "DistrictId");

            migrationBuilder.RenameIndex(
                name: "IX_Items_DisrtictId",
                table: "Items",
                newName: "IX_Items_DistrictId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Districts_DistrictId",
                table: "Items",
                column: "DistrictId",
                principalTable: "Districts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Districts_DistrictId",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "DistrictId",
                table: "Items",
                newName: "DisrtictId");

            migrationBuilder.RenameIndex(
                name: "IX_Items_DistrictId",
                table: "Items",
                newName: "IX_Items_DisrtictId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Districts_DisrtictId",
                table: "Items",
                column: "DisrtictId",
                principalTable: "Districts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
