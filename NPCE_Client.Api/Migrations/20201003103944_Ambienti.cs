using Microsoft.EntityFrameworkCore.Migrations;

namespace NPCE_Client.Api.Migrations
{
    public partial class Ambienti : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ambienti",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(nullable: false),
                    customerid = table.Column<string>(nullable: true),
                    costcenter = table.Column<string>(nullable: true),
                    billingcenter = table.Column<string>(nullable: true),
                    idsender = table.Column<string>(nullable: true),
                    sendersystem = table.Column<string>(nullable: false),
                    smuser = table.Column<string>(nullable: false),
                    contracttype = table.Column<string>(nullable: true),
                    usertype = table.Column<string>(nullable: true),
                    codicefiscale = table.Column<string>(nullable: true),
                    partitaiva = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    LolUri = table.Column<string>(nullable: true),
                    RolUri = table.Column<string>(nullable: true),
                    ColUri = table.Column<string>(nullable: true),
                    MolUri = table.Column<string>(nullable: true),
                    Contratto = table.Column<string>(nullable: true),
                    VolUri = table.Column<string>(nullable: true),
                    contractid = table.Column<string>(nullable: true),
                    customer = table.Column<string>(nullable: true),
                    IsPil = table.Column<bool>(nullable: false),
                    ContrattoMOL = table.Column<string>(nullable: true),
                    ContrattoCOL = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ambienti", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ambienti");
        }
    }
}
