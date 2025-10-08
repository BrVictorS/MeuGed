using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGD.Migrations
{
    public partial class indexacaotable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Indexacao",
                columns: table => new
                {
                    MetadadoTipoDocId = table.Column<int>(type: "int", nullable: false),
                    DocumentoId = table.Column<int>(type: "int", nullable: false),
                    LoteId = table.Column<int>(type: "int", nullable: false),
                    Valor = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Indexacao", x => new { x.MetadadoTipoDocId, x.DocumentoId, x.LoteId });
                    table.ForeignKey(
                        name: "FK_Indexacao_Documentos_DocumentoId",
                        column: x => x.DocumentoId,
                        principalTable: "Documentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Indexacao_Lote_LoteId",
                        column: x => x.LoteId,
                        principalTable: "Lote",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Indexacao_MetadadosTipoDoc_MetadadoTipoDocId",
                        column: x => x.MetadadoTipoDocId,
                        principalTable: "MetadadosTipoDoc",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Indexacao_DocumentoId",
                table: "Indexacao",
                column: "DocumentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Indexacao_LoteId",
                table: "Indexacao",
                column: "LoteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Indexacao");
        }
    }
}
