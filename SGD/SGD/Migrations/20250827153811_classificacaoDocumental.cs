using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGD.Migrations
{
    public partial class classificacaoDocumental : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Funcao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Funcao = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Funcao", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subfuncao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subfuncao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FuncaoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subfuncao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subfuncao_Funcao_FuncaoId",
                        column: x => x.FuncaoId,
                        principalTable: "Funcao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Atividade",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Atividade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubfuncaoModelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Atividade", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Atividade_Subfuncao_SubfuncaoModelId",
                        column: x => x.SubfuncaoModelId,
                        principalTable: "Subfuncao",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Especie",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Especie = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AtividadeModelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Especie", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Especie_Atividade_AtividadeModelId",
                        column: x => x.AtividadeModelId,
                        principalTable: "Atividade",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EspecieTipoDoc",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EspecieId = table.Column<int>(type: "int", nullable: false),
                    TipoDocumentalId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EspecieTipoDoc", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EspecieTipoDoc_Especie_EspecieId",
                        column: x => x.EspecieId,
                        principalTable: "Especie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EspecieTipoDoc_TipoDocumental_TipoDocumentalId",
                        column: x => x.TipoDocumentalId,
                        principalTable: "TipoDocumental",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Atividade_SubfuncaoModelId",
                table: "Atividade",
                column: "SubfuncaoModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Especie_AtividadeModelId",
                table: "Especie",
                column: "AtividadeModelId");

            migrationBuilder.CreateIndex(
                name: "IX_EspecieTipoDoc_EspecieId",
                table: "EspecieTipoDoc",
                column: "EspecieId");

            migrationBuilder.CreateIndex(
                name: "IX_EspecieTipoDoc_TipoDocumentalId",
                table: "EspecieTipoDoc",
                column: "TipoDocumentalId");

            migrationBuilder.CreateIndex(
                name: "IX_Subfuncao_FuncaoId",
                table: "Subfuncao",
                column: "FuncaoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EspecieTipoDoc");

            migrationBuilder.DropTable(
                name: "Especie");

            migrationBuilder.DropTable(
                name: "Atividade");

            migrationBuilder.DropTable(
                name: "Subfuncao");

            migrationBuilder.DropTable(
                name: "Funcao");
        }
    }
}
