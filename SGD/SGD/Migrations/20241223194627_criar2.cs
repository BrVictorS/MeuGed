using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGD.Migrations
{
    public partial class criar2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FluxoLote_UsuarioModel_UsuarioId",
                table: "FluxoLote");

            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioPermissoes_UsuarioModel_UsuarioId",
                table: "UsuarioPermissoes");

            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioProjetoModel_UsuarioModel_ProjetoId",
                table: "UsuarioProjetoModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsuarioModel",
                table: "UsuarioModel");

            migrationBuilder.RenameTable(
                name: "UsuarioModel",
                newName: "Usuarios");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Usuarios",
                table: "Usuarios",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FluxoLote_Usuarios_UsuarioId",
                table: "FluxoLote",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioPermissoes_Usuarios_UsuarioId",
                table: "UsuarioPermissoes",
                column: "UsuarioId",
                principalTable: "Usuarios",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FluxoLote_Usuarios_UsuarioId",
                table: "FluxoLote");

            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioPermissoes_Usuarios_UsuarioId",
                table: "UsuarioPermissoes");

            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioProjetoModel_Usuarios_ProjetoId",
                table: "UsuarioProjetoModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Usuarios",
                table: "Usuarios");

            migrationBuilder.RenameTable(
                name: "Usuarios",
                newName: "UsuarioModel");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsuarioModel",
                table: "UsuarioModel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FluxoLote_UsuarioModel_UsuarioId",
                table: "FluxoLote",
                column: "UsuarioId",
                principalTable: "UsuarioModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioPermissoes_UsuarioModel_UsuarioId",
                table: "UsuarioPermissoes",
                column: "UsuarioId",
                principalTable: "UsuarioModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioProjetoModel_UsuarioModel_ProjetoId",
                table: "UsuarioProjetoModel",
                column: "ProjetoId",
                principalTable: "UsuarioModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
