using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGD.Migrations
{
    public partial class uptFluxo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FluxoLote_LoteId",
                table: "FluxoLote");

            migrationBuilder.CreateIndex(
                name: "IX_FluxoLote_LoteId_SituacaoId",
                table: "FluxoLote",
                columns: new[] { "LoteId", "SituacaoId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FluxoLote_LoteId_SituacaoId",
                table: "FluxoLote");

            migrationBuilder.CreateIndex(
                name: "IX_FluxoLote_LoteId",
                table: "FluxoLote",
                column: "LoteId");
        }
    }
}
