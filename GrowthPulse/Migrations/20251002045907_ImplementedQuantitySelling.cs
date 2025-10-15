using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GrowthPulse.Migrations
{
    public partial class ImplementedQuantitySelling : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Listings_Plants_PlantId",
                table: "Listings");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Listings_ListingId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ListingId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Listings_PlantId",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "ListingId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "IsSold",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "ListingDate",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "PlantId",
                table: "Listings");

            migrationBuilder.RenameColumn(
                name: "FinalPrice",
                table: "Orders",
                newName: "TotalPrice");

            migrationBuilder.AddColumn<int>(
                name: "ListingId",
                table: "Plants",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Listings",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "Listings",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StockQuantity",
                table: "Listings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<int>(nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OrderId = table.Column<int>(nullable: false),
                    ListingId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Listings_ListingId",
                        column: x => x.ListingId,
                        principalTable: "Listings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Plants_ListingId",
                table: "Plants",
                column: "ListingId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ListingId",
                table: "OrderItems",
                column: "ListingId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Plants_Listings_ListingId",
                table: "Plants",
                column: "ListingId",
                principalTable: "Listings",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Plants_Listings_ListingId",
                table: "Plants");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_Plants_ListingId",
                table: "Plants");

            migrationBuilder.DropColumn(
                name: "ListingId",
                table: "Plants");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "StockQuantity",
                table: "Listings");

            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "Orders",
                newName: "FinalPrice");

            migrationBuilder.AddColumn<int>(
                name: "ListingId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsSold",
                table: "Listings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ListingDate",
                table: "Listings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "PlantId",
                table: "Listings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ListingId",
                table: "Orders",
                column: "ListingId");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_PlantId",
                table: "Listings",
                column: "PlantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_Plants_PlantId",
                table: "Listings",
                column: "PlantId",
                principalTable: "Plants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Listings_ListingId",
                table: "Orders",
                column: "ListingId",
                principalTable: "Listings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
