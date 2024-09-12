using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeatherReport.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddPasswordSalt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Subscribers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Subscribers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserRole",
                table: "Subscribers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Subscribers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "SubscriptionDate", "UserName", "UserRole" },
                values: new object[] { "", new DateTime(2024, 9, 12, 15, 45, 52, 333, DateTimeKind.Utc).AddTicks(4690), "admin", "Admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Subscribers");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Subscribers");

            migrationBuilder.DropColumn(
                name: "UserRole",
                table: "Subscribers");

            migrationBuilder.UpdateData(
                table: "Subscribers",
                keyColumn: "Id",
                keyValue: 1,
                column: "SubscriptionDate",
                value: new DateTime(2024, 9, 12, 15, 31, 2, 66, DateTimeKind.Utc).AddTicks(2550));
        }
    }
}
