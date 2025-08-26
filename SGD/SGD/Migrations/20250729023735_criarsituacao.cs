using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGD.Migrations
{
    public partial class criarsituacao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Situacao",
                table: "FluxoLote",
                newName: "SituacaoId");

            migrationBuilder.CreateTable(
                name: "Situacoes",
                columns: table => new
                {
                    IdSituacao = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Caminho = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Situacoes", x => x.IdSituacao);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FluxoLote_SituacaoId",
                table: "FluxoLote",
                column: "SituacaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_FluxoLote_Situacoes_SituacaoId",
                table: "FluxoLote",
                column: "SituacaoId",
                principalTable: "Situacoes",
                principalColumn: "IdSituacao",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FluxoLote_Situacoes_SituacaoId",
                table: "FluxoLote");

            migrationBuilder.DropTable(
                name: "Situacoes");

            migrationBuilder.DropIndex(
                name: "IX_FluxoLote_SituacaoId",
                table: "FluxoLote");

            migrationBuilder.RenameColumn(
                name: "SituacaoId",
                table: "FluxoLote",
                newName: "Situacao");
        }
    }
}
