using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PartyInvitationApp.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Parties",
                columns: new[] { "Id", "DateOfParty", "Description", "Location" },
                values: new object[] { 11, new DateTime(2025, 3, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cricket Champions Trophy", "Dubai International Stadium" });

            migrationBuilder.InsertData(
                table: "Invitations",
                columns: new[] { "Id", "GuestEmail", "GuestName", "PartyId", "Status" },
                values: new object[,]
                {
                    { 20, "jpandit5253@conestogac.on.ca", "Virat Kohli", 11, 0 },
                    { 21, "jpandit5253@conestogac.on.ca", "MS Dhoni", 11, 0 },
                    { 22, "jpandit5253@conestogac.on.ca", "Rohit Sharma", 11, 0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Invitations",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Invitations",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Invitations",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Parties",
                keyColumn: "Id",
                keyValue: 11);
        }
    }
}
