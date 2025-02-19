using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessIntelligence.Migrations
{
    public partial class AggiuntaEmailUtente : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Cognome",
                table: "Utenti",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Utenti",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cognome",
                table: "Utenti");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Utenti");
        }
    }
}
