using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Realestate.Migrations
{
    /// <inheritdoc />
    public partial class Status : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Items",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Items_StatusId",
                table: "Items",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Statuses_StatusId",
                table: "Items",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Statuses_StatusId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_StatusId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Items");
        }
    }
}
