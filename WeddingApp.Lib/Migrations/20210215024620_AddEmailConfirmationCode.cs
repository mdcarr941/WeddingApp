using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WeddingApp.Lib.Migrations
{
    public partial class AddEmailConfirmationCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmailConfirmationCodes",
                columns: table => new
                {
                    Code = table.Column<Guid>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailConfirmationCodes", x => x.Code);
                    table.ForeignKey(
                        name: "FK_EmailConfirmationCodes_Rsvps_Email",
                        column: x => x.Email,
                        principalTable: "Rsvps",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmailConfirmationCodes_Email",
                table: "EmailConfirmationCodes",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailConfirmationCodes");
        }
    }
}
