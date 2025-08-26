using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGD.Migrations
{
    public partial class CorrigeRelacionamento : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioProjetos_Projeto_UsuarioId",
                table: "UsuarioProjetos");

            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioProjetos_Usuarios_ProjetoId",
                table: "UsuarioProjetos");

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioProjetos_Projeto_ProjetoId",
                table: "UsuarioProjetos",
                column: "ProjetoId",
                principalTable: "Projeto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioProjetos_Usuarios_UsuarioId",
                table: "UsuarioProjetos",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioProjetos_Projeto_ProjetoId",
                table: "UsuarioProjetos");

            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioProjetos_Usuarios_UsuarioId",
                table: "UsuarioProjetos");

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioProjetos_Projeto_UsuarioId",
                table: "UsuarioProjetos",
                column: "UsuarioId",
                principalTable: "Projeto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioProjetos_Usuarios_ProjetoId",
                table: "UsuarioProjetos",
                column: "ProjetoId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
