using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGD.Migrations
{
    public partial class metadados : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Metadados_TipoDocumental_TipoDocumentalModelId",
                table: "Metadados");

            migrationBuilder.DropIndex(
                name: "IX_Metadados_TipoDocumentalModelId",
                table: "Metadados");

            migrationBuilder.DropColumn(
                name: "TipoDocumentalModelId",
                table: "Metadados");

            migrationBuilder.CreateTable(
                name: "MetadadosTipoDoc",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MetadadoId = table.Column<int>(type: "int", nullable: false),
                    TipoDocumentalId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetadadosTipoDoc", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MetadadosTipoDoc_Metadados_MetadadoId",
                        column: x => x.MetadadoId,
                        principalTable: "Metadados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MetadadosTipoDoc_TipoDocumental_TipoDocumentalId",
                        column: x => x.TipoDocumentalId,
                        principalTable: "TipoDocumental",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MetadadosTipoDoc_MetadadoId",
                table: "MetadadosTipoDoc",
                column: "MetadadoId");

            migrationBuilder.CreateIndex(
                name: "IX_MetadadosTipoDoc_TipoDocumentalId",
                table: "MetadadosTipoDoc",
                column: "TipoDocumentalId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MetadadosTipoDoc");

            migrationBuilder.AddColumn<int>(
                name: "TipoDocumentalModelId",
                table: "Metadados",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Metadados_TipoDocumentalModelId",
                table: "Metadados",
                column: "TipoDocumentalModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Metadados_TipoDocumental_TipoDocumentalModelId",
                table: "Metadados",
                column: "TipoDocumentalModelId",
                principalTable: "TipoDocumental",
                principalColumn: "Id");
        }
    }
}
