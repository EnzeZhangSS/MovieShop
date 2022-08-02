using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class RatingSmallChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Rating",
                table: "Reviews",
                type: "decimal(4,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(3,2)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Rating",
                table: "Reviews",
                type: "decimal(3,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(4,2)");
        }
    }
}
