using Microsoft.EntityFrameworkCore.Migrations;

namespace NPCE_Client.Api.Migrations
{
    public partial class TipoServizio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TipiServizio",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipiServizio", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "TipiServizio",
                columns: new[] { "Id", "Description" },
                values: new object[,]
                {
                    { 0, "Posta 1" },
                    { 1, "Posta 4" },
                    { 2, "Raccomandata" },
                    { 3, "PostaContest 1" },
                    { 4, "PostaContest 4" },
                    { 5, "RaccomandataMarket 1" },
                    { 6, "RaccomandataMarket 4" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TipiServizio");
        }
    }
}
