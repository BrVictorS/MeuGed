using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGD.Migrations
{
    public partial class criar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Permissoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissoes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projeto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projeto", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoDocumental",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoDocumental", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Situação = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lote",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumLote = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Observacao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjetoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lote_Projeto_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "Projeto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjetoPermissaoModel",
                columns: table => new
                {
                    ProjetoId = table.Column<int>(type: "int", nullable: false),
                    PermissaoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjetoPermissaoModel", x => new { x.ProjetoId, x.PermissaoId });
                    table.ForeignKey(
                        name: "FK_ProjetoPermissaoModel_Permissoes_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "Permissoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjetoPermissaoModel_Projeto_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "Projeto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Metadados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Valor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoDocumentalModelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Metadados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Metadados_TipoDocumental_TipoDocumentalModelId",
                        column: x => x.TipoDocumentalModelId,
                        principalTable: "TipoDocumental",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UsuarioPermissoes",
                columns: table => new
                {
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    PermissaoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioPermissoes", x => new { x.UsuarioId, x.PermissaoId });
                    table.ForeignKey(
                        name: "FK_UsuarioPermissoes_Permissoes_PermissaoId",
                        column: x => x.PermissaoId,
                        principalTable: "Permissoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuarioPermissoes_UsuarioModel_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "UsuarioModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioProjetoModel",
                columns: table => new
                {
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    ProjetoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioProjetoModel", x => new { x.ProjetoId, x.UsuarioId });
                    table.ForeignKey(
                        name: "FK_UsuarioProjetoModel_Projeto_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Projeto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuarioProjetoModel_UsuarioModel_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "UsuarioModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FluxoLote",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Situacao = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    DtInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DtFim = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LoteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FluxoLote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FluxoLote_Lote_LoteId",
                        column: x => x.LoteId,
                        principalTable: "Lote",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FluxoLote_UsuarioModel_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "UsuarioModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Protocolos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Etiqueta = table.Column<long>(type: "bigint", nullable: false),
                    LoteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Protocolos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Protocolos_Lote_LoteId",
                        column: x => x.LoteId,
                        principalTable: "Lote",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Documentos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoDocId = table.Column<int>(type: "int", nullable: false),
                    ProtocoloId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documentos_Protocolos_ProtocoloId",
                        column: x => x.ProtocoloId,
                        principalTable: "Protocolos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Documentos_TipoDocumental_TipoDocId",
                        column: x => x.TipoDocId,
                        principalTable: "TipoDocumental",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Documentos_ProtocoloId",
                table: "Documentos",
                column: "ProtocoloId");

            migrationBuilder.CreateIndex(
                name: "IX_Documentos_TipoDocId",
                table: "Documentos",
                column: "TipoDocId");

            migrationBuilder.CreateIndex(
                name: "IX_FluxoLote_LoteId",
                table: "FluxoLote",
                column: "LoteId");

            migrationBuilder.CreateIndex(
                name: "IX_FluxoLote_UsuarioId",
                table: "FluxoLote",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Lote_ProjetoId",
                table: "Lote",
                column: "ProjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_Metadados_TipoDocumentalModelId",
                table: "Metadados",
                column: "TipoDocumentalModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Protocolos_LoteId",
                table: "Protocolos",
                column: "LoteId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioPermissoes_PermissaoId",
                table: "UsuarioPermissoes",
                column: "PermissaoId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioProjetoModel_UsuarioId",
                table: "UsuarioProjetoModel",
                column: "UsuarioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Documentos");

            migrationBuilder.DropTable(
                name: "FluxoLote");

            migrationBuilder.DropTable(
                name: "Metadados");

            migrationBuilder.DropTable(
                name: "ProjetoPermissaoModel");

            migrationBuilder.DropTable(
                name: "UsuarioPermissoes");

            migrationBuilder.DropTable(
                name: "UsuarioProjetoModel");

            migrationBuilder.DropTable(
                name: "Protocolos");

            migrationBuilder.DropTable(
                name: "TipoDocumental");

            migrationBuilder.DropTable(
                name: "Permissoes");

            migrationBuilder.DropTable(
                name: "UsuarioModel");

            migrationBuilder.DropTable(
                name: "Lote");

            migrationBuilder.DropTable(
                name: "Projeto");
        }
    }
}
