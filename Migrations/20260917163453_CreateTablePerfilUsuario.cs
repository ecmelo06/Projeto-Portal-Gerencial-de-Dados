using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.Migrations
{
    /// <inheritdoc />
    public partial class CreateTablePerfilUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AGERGS",
                table: "Perfil");

            migrationBuilder.DropColumn(
                name: "Admin",
                table: "Perfil");

            migrationBuilder.DropColumn(
                name: "Comum",
                table: "Perfil");

            migrationBuilder.RenameColumn(
                name: "Externo",
                table: "Perfil",
                newName: "Nome");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "Perfil",
                newName: "Externo");

            migrationBuilder.AddColumn<string>(
                name: "AGERGS",
                table: "Perfil",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Admin",
                table: "Perfil",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Comum",
                table: "Perfil",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
