using Microsoft.EntityFrameworkCore.Migrations;

namespace NPCE_Client.Api.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Anagrafiche",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cognome = table.Column<string>(nullable: false),
                    Nome = table.Column<string>(nullable: true),
                    RagioneSociale = table.Column<string>(nullable: true),
                    ComplementoNominativo = table.Column<string>(nullable: true),
                    DUG = table.Column<string>(nullable: true),
                    Toponimo = table.Column<string>(nullable: true),
                    Esponente = table.Column<string>(nullable: true),
                    NumeroCivico = table.Column<string>(nullable: true),
                    ComplementoIndirizzo = table.Column<string>(nullable: true),
                    Frazione = table.Column<string>(nullable: true),
                    Citta = table.Column<string>(nullable: true),
                    Cap = table.Column<string>(nullable: true),
                    Telefono = table.Column<string>(nullable: true),
                    CasellaPostale = table.Column<string>(nullable: true),
                    Provincia = table.Column<string>(nullable: true),
                    Stato = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Anagrafiche", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Anagrafiche");
        }
    }
}
