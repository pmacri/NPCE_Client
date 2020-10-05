using Microsoft.EntityFrameworkCore.Migrations;

namespace NPCE_Client.Api.Migrations
{
    public partial class ambienteIdNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Servizi_Ambienti_AmbienteId",
                table: "Servizi");

            migrationBuilder.AlterColumn<int>(
                name: "AmbienteId",
                table: "Servizi",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Servizi_Ambienti_AmbienteId",
                table: "Servizi",
                column: "AmbienteId",
                principalTable: "Ambienti",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Servizi_Ambienti_AmbienteId",
                table: "Servizi");

            migrationBuilder.AlterColumn<int>(
                name: "AmbienteId",
                table: "Servizi",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Servizi_Ambienti_AmbienteId",
                table: "Servizi",
                column: "AmbienteId",
                principalTable: "Ambienti",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
