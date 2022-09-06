using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Usuarios_identity.Migrations
{
    public partial class Añadimosedad : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Edad",
                table: "AspNetUsers",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Edad",
                table: "AspNetUsers");
        }
    }
}
