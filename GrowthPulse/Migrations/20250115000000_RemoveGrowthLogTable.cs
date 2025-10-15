using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GrowthPulse.Migrations
{
    public partial class RemoveGrowthLogTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GrowthLogs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GrowthLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LogDate = table.Column<DateTime>(nullable: false),
                    Height = table.Column<double>(nullable: false),
                    HealthStatus = table.Column<string>(maxLength: 200, nullable: true),
                    Observations = table.Column<string>(maxLength: 500, nullable: true),
                    PhotoUrl = table.Column<string>(nullable: true),
                    PlantId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrowthLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GrowthLogs_Plants_PlantId",
                        column: x => x.PlantId,
                        principalTable: "Plants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GrowthLogs_PlantId",
                table: "GrowthLogs",
                column: "PlantId");
        }
    }
}
