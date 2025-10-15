using Microsoft.EntityFrameworkCore.Migrations;

namespace GrowthPulse.Migrations
{
    public partial class Staff_Check : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ForSale",
                table: "GrowthLogs",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ForSale",
                table: "GrowthLogs");
        }
    }
}
