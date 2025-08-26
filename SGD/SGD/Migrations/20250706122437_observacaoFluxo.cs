using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGD.Migrations
{
    public partial class observacaoFluxo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Observacao",
                table: "FluxoLote",
                type: "nvarchar(10)",
                nullable: true,
                defaultValue: null);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Observacao",
                table: "FluxoLote");
        }
    }
}
