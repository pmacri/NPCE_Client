using Microsoft.EntityFrameworkCore.Migrations;

namespace NPCE_Client.Api.Migrations
{
    public partial class StatoServizio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatoServizioId",
                table: "Servizi",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "StatoServizio",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatoServizio", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "StatoServizio",
                columns: new[] { "Id", "Description" },
                values: new object[] { 0, "Da Inviare" });

            migrationBuilder.InsertData(
                table: "StatoServizio",
                columns: new[] { "Id", "Description" },
                values: new object[] { 1, "Inviato" });

            migrationBuilder.InsertData(
                table: "StatoServizio",
                columns: new[] { "Id", "Description" },
                values: new object[] { 2, "Confermato" });

            migrationBuilder.CreateIndex(
                name: "IX_Servizi_StatoServizioId",
                table: "Servizi",
                column: "StatoServizioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Servizi_StatoServizio_StatoServizioId",
                table: "Servizi",
                column: "StatoServizioId",
                principalTable: "StatoServizio",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Servizi_StatoServizio_StatoServizioId",
                table: "Servizi");

            migrationBuilder.DropTable(
                name: "StatoServizio");

            migrationBuilder.DropIndex(
                name: "IX_Servizi_StatoServizioId",
                table: "Servizi");

            migrationBuilder.DropColumn(
                name: "StatoServizioId",
                table: "Servizi");
        }
    }
}
