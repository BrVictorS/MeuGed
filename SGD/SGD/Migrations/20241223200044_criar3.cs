using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGD.Migrations
{
    public partial class criar3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioProjetoModel_Projeto_UsuarioId",
                table: "UsuarioProjetoModel");

            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioProjetoModel_Usuarios_ProjetoId",
                table: "UsuarioProjetoModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsuarioProjetoModel",
                table: "UsuarioProjetoModel");

            migrationBuilder.RenameTable(
                name: "UsuarioProjetoModel",
                newName: "UsuarioProjetos");

            migrationBuilder.RenameIndex(
                name: "IX_UsuarioProjetoModel_UsuarioId",
                table: "UsuarioProjetos",
                newName: "IX_UsuarioProjetos_UsuarioId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsuarioProjetos",
                table: "UsuarioProjetos",
                columns: new[] { "ProjetoId", "UsuarioId" });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioProjetos_Projeto_UsuarioId",
                table: "UsuarioProjetos");

            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioProjetos_Usuarios_ProjetoId",
                table: "UsuarioProjetos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsuarioProjetos",
                table: "UsuarioProjetos");

            migrationBuilder.RenameTable(
                name: "UsuarioProjetos",
                newName: "UsuarioProjetoModel");

            migrationBuilder.RenameIndex(
                name: "IX_UsuarioProjetos_UsuarioId",
                table: "UsuarioProjetoModel",
                newName: "IX_UsuarioProjetoModel_UsuarioId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsuarioProjetoModel",
                table: "UsuarioProjetoModel",
                columns: new[] { "ProjetoId", "UsuarioId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioProjetoModel_Projeto_UsuarioId",
                table: "UsuarioProjetoModel",
                column: "UsuarioId",
                principalTable: "Projeto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioProjetoModel_Usuarios_ProjetoId",
                table: "UsuarioProjetoModel",
                column: "ProjetoId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
