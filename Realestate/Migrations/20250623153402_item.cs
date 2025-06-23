using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Realestate.Migrations
{
    /// <inheritdoc />
    public partial class item : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    DisrtictId = table.Column<int>(type: "int", nullable: false),
                    MyTypeId = table.Column<int>(type: "int", nullable: false),
                    PropertyTypeId = table.Column<int>(type: "int", nullable: false),
                    AdvertiseNo = table.Column<int>(type: "int", nullable: false),
                    AdNo = table.Column<int>(type: "int", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Soum = table.Column<double>(type: "float", nullable: false),
                    Limit = table.Column<double>(type: "float", nullable: false),
                    StreetWidth = table.Column<double>(type: "float", nullable: false),
                    Space = table.Column<double>(type: "float", nullable: false),
                    PricePerMeter = table.Column<double>(type: "float", nullable: false),
                    DescriptionEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HasUnits = table.Column<bool>(type: "bit", nullable: false),
                    RengeFrom = table.Column<double>(type: "float", nullable: false),
                    RangeTo = table.Column<double>(type: "float", nullable: false),
                    HasPassword = table.Column<bool>(type: "bit", nullable: false),
                    HashPassword = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Items_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Items_Districts_DisrtictId",
                        column: x => x.DisrtictId,
                        principalTable: "Districts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Items_PropertyTypes_PropertyTypeId",
                        column: x => x.PropertyTypeId,
                        principalTable: "PropertyTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Items_Types_MyTypeId",
                        column: x => x.MyTypeId,
                        principalTable: "Types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Items_CategoryId",
                table: "Items",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_CityId",
                table: "Items",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_DisrtictId",
                table: "Items",
                column: "DisrtictId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_MyTypeId",
                table: "Items",
                column: "MyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_PropertyTypeId",
                table: "Items",
                column: "PropertyTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Items");
        }
    }
}
