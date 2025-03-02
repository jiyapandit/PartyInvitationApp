using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PartyInvitationApp.Migrations
{
    /// <inheritdoc />
    public partial class RenameEventDateToDateOfParty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EventDate",
                table: "Parties",
                newName: "DateOfParty");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateOfParty",
                table: "Parties",
                newName: "EventDate");
        }
    }
}
