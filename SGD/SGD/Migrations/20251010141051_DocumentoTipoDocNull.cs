using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGD.Migrations
{
    public partial class DocumentoTipoDocNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
           name: "TipoDocId",
           table: "Documentos",
           nullable: true
           );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
