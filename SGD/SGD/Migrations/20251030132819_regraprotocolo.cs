using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGD.Migrations
{
    public partial class regraprotocolo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Protocolos_Etiqueta_LoteId",
                table: "Protocolos",
                columns: new[] { "Etiqueta", "LoteId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Protocolos_Etiqueta_LoteId",
                table: "Protocolos");
        }
    }
}
