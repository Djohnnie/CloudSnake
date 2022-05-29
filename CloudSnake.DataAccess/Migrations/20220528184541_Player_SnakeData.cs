using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CloudSnake.DataAccess.Migrations
{
    public partial class Player_SnakeData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SnakeData",
                table: "PLAYERS",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SnakeData",
                table: "PLAYERS");
        }
    }
}
