using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessIntelligence.Migrations
{
    public partial class AggiungiEta : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Età",
                table: "Utenti",
                newName: "Eta");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Eta",
                table: "Utenti",
                newName: "Età");
        }
    }
}
