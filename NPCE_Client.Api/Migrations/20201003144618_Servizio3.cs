using Microsoft.EntityFrameworkCore.Migrations;

namespace NPCE_Client.Api.Migrations
{
    public partial class Servizio3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Anagrafiche_Servizi_ServizioId",
                table: "Anagrafiche");

            migrationBuilder.DropForeignKey(
                name: "FK_Documenti_Servizi_ServizioId",
                table: "Documenti");

            migrationBuilder.DropForeignKey(
                name: "FK_Servizi_Anagrafiche_MittenteId",
                table: "Servizi");

            migrationBuilder.DropIndex(
                name: "IX_Servizi_MittenteId",
                table: "Servizi");

            migrationBuilder.DropIndex(
                name: "IX_Documenti_ServizioId",
                table: "Documenti");

            migrationBuilder.DropIndex(
                name: "IX_Anagrafiche_ServizioId",
                table: "Anagrafiche");

            migrationBuilder.DropColumn(
                name: "MittenteId",
                table: "Servizi");

            migrationBuilder.DropColumn(
                name: "ServizioId",
                table: "Documenti");

            migrationBuilder.DropColumn(
                name: "ServizioId",
                table: "Anagrafiche");

            migrationBuilder.AddColumn<int>(
                name: "DocumentoId",
                table: "Servizi",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ServizioAnagrafica",
                columns: table => new
                {
                    ServizioId = table.Column<int>(nullable: false),
                    AnagraficaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServizioAnagrafica", x => new { x.ServizioId, x.AnagraficaId });
                    table.ForeignKey(
                        name: "FK_ServizioAnagrafica_Anagrafiche_AnagraficaId",
                        column: x => x.AnagraficaId,
                        principalTable: "Anagrafiche",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServizioAnagrafica_Servizi_ServizioId",
                        column: x => x.ServizioId,
                        principalTable: "Servizi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Servizi_DocumentoId",
                table: "Servizi",
                column: "DocumentoId");

            migrationBuilder.CreateIndex(
                name: "IX_ServizioAnagrafica_AnagraficaId",
                table: "ServizioAnagrafica",
                column: "AnagraficaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Servizi_Documenti_DocumentoId",
                table: "Servizi",
                column: "DocumentoId",
                principalTable: "Documenti",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Servizi_Documenti_DocumentoId",
                table: "Servizi");

            migrationBuilder.DropTable(
                name: "ServizioAnagrafica");

            migrationBuilder.DropIndex(
                name: "IX_Servizi_DocumentoId",
                table: "Servizi");

            migrationBuilder.DropColumn(
                name: "DocumentoId",
                table: "Servizi");

            migrationBuilder.AddColumn<int>(
                name: "MittenteId",
                table: "Servizi",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ServizioId",
                table: "Documenti",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ServizioId",
                table: "Anagrafiche",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Servizi_MittenteId",
                table: "Servizi",
                column: "MittenteId");

            migrationBuilder.CreateIndex(
                name: "IX_Documenti_ServizioId",
                table: "Documenti",
                column: "ServizioId");

            migrationBuilder.CreateIndex(
                name: "IX_Anagrafiche_ServizioId",
                table: "Anagrafiche",
                column: "ServizioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Anagrafiche_Servizi_ServizioId",
                table: "Anagrafiche",
                column: "ServizioId",
                principalTable: "Servizi",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Documenti_Servizi_ServizioId",
                table: "Documenti",
                column: "ServizioId",
                principalTable: "Servizi",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Servizi_Anagrafiche_MittenteId",
                table: "Servizi",
                column: "MittenteId",
                principalTable: "Anagrafiche",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
