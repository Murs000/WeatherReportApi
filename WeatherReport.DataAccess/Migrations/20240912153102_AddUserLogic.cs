using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeatherReport.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddUserLogic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Subscribers",
                keyColumn: "Id",
                keyValue: 1,
                column: "SubscriptionDate",
                value: new DateTime(2024, 9, 12, 15, 31, 2, 66, DateTimeKind.Utc).AddTicks(2550));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Subscribers",
                keyColumn: "Id",
                keyValue: 1,
                column: "SubscriptionDate",
                value: new DateTime(2024, 9, 12, 15, 14, 48, 902, DateTimeKind.Utc).AddTicks(3510));
        }
    }
}
