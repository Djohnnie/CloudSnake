using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CloudSnake.DataAccess.Migrations
{
    public partial class Game_IsReady : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsReady",
                table: "GAMES",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReady",
                table: "GAMES");
        }
    }
}
