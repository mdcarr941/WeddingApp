using Microsoft.EntityFrameworkCore.Migrations;

namespace WeddingApp.Lib.Migrations
{
    public partial class AddRsvpAccepted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Accepted",
                table: "Rsvps",
                type: "INTEGER",
                nullable: false,
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Accepted",
                table: "Rsvps");
        }
    }
}
