using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Usuarios_identity.Migrations
{
    public partial class data : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ComentarioAlAdministrador",
                table: "ComentarioAlAdministrador");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "ComentarioAlAdministrador",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "IdNo",
                table: "ComentarioAlAdministrador",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ComentarioAlAdministrador",
                table: "ComentarioAlAdministrador",
                column: "IdNo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ComentarioAlAdministrador",
                table: "ComentarioAlAdministrador");

            migrationBuilder.DropColumn(
                name: "IdNo",
                table: "ComentarioAlAdministrador");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "ComentarioAlAdministrador",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ComentarioAlAdministrador",
                table: "ComentarioAlAdministrador",
                column: "Id");
        }
    }
}
