using Microsoft.EntityFrameworkCore.Migrations;

namespace NPCE_Client.Api.Migrations
{
    public partial class Servizio2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ServizioId",
                table: "Documenti",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ServizioId",
                table: "Anagrafiche",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Servizi",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AvvisoRicevimento = table.Column<bool>(nullable: false),
                    ArchiviazioneDigitale = table.Column<bool>(nullable: false),
                    Autoconferma = table.Column<bool>(nullable: false),
                    IdRichiesta = table.Column<string>(nullable: true),
                    AttestazioneConsegna = table.Column<bool>(nullable: false),
                    FronteRetro = table.Column<bool>(nullable: false),
                    SecondoTentativoRecapito = table.Column<bool>(nullable: false),
                    Colore = table.Column<bool>(nullable: false),
                    GuidUtente = table.Column<string>(nullable: true),
                    IdOrdine = table.Column<string>(nullable: true),
                    TipoArchiviazione = table.Column<string>(nullable: true),
                    AnniArchiviazione = table.Column<int>(nullable: false),
                    TipoServizioId = table.Column<int>(nullable: true),
                    AmbienteId = table.Column<int>(nullable: false),
                    MittenteId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servizi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Servizi_Ambienti_AmbienteId",
                        column: x => x.AmbienteId,
                        principalTable: "Ambienti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Servizi_Anagrafiche_MittenteId",
                        column: x => x.MittenteId,
                        principalTable: "Anagrafiche",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Servizi_TipiServizio_TipoServizioId",
                        column: x => x.TipoServizioId,
                        principalTable: "TipiServizio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Documenti_ServizioId",
                table: "Documenti",
                column: "ServizioId");

            migrationBuilder.CreateIndex(
                name: "IX_Anagrafiche_ServizioId",
                table: "Anagrafiche",
                column: "ServizioId");

            migrationBuilder.CreateIndex(
                name: "IX_Servizi_AmbienteId",
                table: "Servizi",
                column: "AmbienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Servizi_MittenteId",
                table: "Servizi",
                column: "MittenteId");

            migrationBuilder.CreateIndex(
                name: "IX_Servizi_TipoServizioId",
                table: "Servizi",
                column: "TipoServizioId");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Anagrafiche_Servizi_ServizioId",
                table: "Anagrafiche");

            migrationBuilder.DropForeignKey(
                name: "FK_Documenti_Servizi_ServizioId",
                table: "Documenti");

            migrationBuilder.DropTable(
                name: "Servizi");

            migrationBuilder.DropIndex(
                name: "IX_Documenti_ServizioId",
                table: "Documenti");

            migrationBuilder.DropIndex(
                name: "IX_Anagrafiche_ServizioId",
                table: "Anagrafiche");

            migrationBuilder.DropColumn(
                name: "ServizioId",
                table: "Documenti");

            migrationBuilder.DropColumn(
                name: "ServizioId",
                table: "Anagrafiche");
        }
    }
}
