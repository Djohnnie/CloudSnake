using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CloudSnake.DataAccess.Migrations
{
    public partial class Game_FoodData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FoodData",
                table: "GAMES",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FoodData",
                table: "GAMES");
        }
    }
}
