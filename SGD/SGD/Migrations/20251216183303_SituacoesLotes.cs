using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGD.Migrations
{
    public partial class SituacoesLotes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Preparado",
                table: "Lote",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Scaneado",
                table: "Lote",
                type: "bit",
                nullable: true);
            migrationBuilder.AddColumn<bool>(
                name: "Importado",
                table: "Lote",
                type: "bit",
                nullable: true);
            migrationBuilder.AddColumn<bool>(
                name: "Verificado",
                table: "Lote",
                type: "bit",
                nullable: true);
            migrationBuilder.AddColumn<bool>(
               name: "Indexado",
               table: "Lote",
               type: "bit",
               nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Exportado",
                table: "Lote",
                type: "bit",
                nullable: true);            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Exportado",
                table: "Lote");

            migrationBuilder.DropColumn(
                name: "Importado",
                table: "Lote");

            migrationBuilder.DropColumn(
                name: "Indexado",
                table: "Lote");

            migrationBuilder.DropColumn(
                name: "Preparado",
                table: "Lote");

            migrationBuilder.DropColumn(
                name: "Scaneado",
                table: "Lote");

            migrationBuilder.DropColumn(
                name: "Verificado",
                table: "Lote");
        }
    }
}
