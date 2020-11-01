using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NPCE_Client.Api.Migrations
{
    public partial class TipoServizio2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataCreazione",
                table: "Servizi",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataCreazione",
                table: "Servizi");
        }
    }
}
