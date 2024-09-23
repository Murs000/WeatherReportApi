using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeatherReport.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddIsActivatedAndRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PasswordSalt",
                table: "Subscribers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Subscribers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt", "SubscriptionDate" },
                values: new object[] { "RLxOtsWawDlESSGFfMzkTYqlW5x11dGfGR0xB2LcRTg=", "40cc50e45cba25c463a4130cd22e7e14", new DateTime(2024, 9, 13, 8, 33, 53, 248, DateTimeKind.Utc).AddTicks(9880) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                table: "Subscribers");

            migrationBuilder.UpdateData(
                table: "Subscribers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "SubscriptionDate" },
                values: new object[] { "", new DateTime(2024, 9, 12, 15, 45, 52, 333, DateTimeKind.Utc).AddTicks(4690) });
        }
    }
}
