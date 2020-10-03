using Microsoft.EntityFrameworkCore.Migrations;

namespace NPCE_Client.Api.Migrations
{
    public partial class DocumentiManyToMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ServizioDocumento",
                columns: table => new
                {
                    ServizioId = table.Column<int>(nullable: false),
                    DocumentoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServizioDocumento", x => new { x.ServizioId, x.DocumentoId });
                    table.ForeignKey(
                        name: "FK_ServizioDocumento_Documenti_DocumentoId",
                        column: x => x.DocumentoId,
                        principalTable: "Documenti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServizioDocumento_Servizi_ServizioId",
                        column: x => x.ServizioId,
                        principalTable: "Servizi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServizioDocumento_DocumentoId",
                table: "ServizioDocumento",
                column: "DocumentoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServizioDocumento");
        }
    }
}
