using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814

namespace GameDay_Sync.Migrations
{
    public partial class SeedDefaultTargets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "targets",
                columns: new[] { "id", "category", "date_added", "is_active", "name", "sportsdb_id" },
                values: new object[,]
                {
                    { 1, "Team", new DateTime(2026, 9, 17, 0, 0, 0, 0, DateTimeKind.Utc), true, "Houston Astros", 135256 },
                    { 2, "Team", new DateTime(2026, 9, 17, 0, 0, 0, 0, DateTimeKind.Utc), true, "Tottenham Hotspur", 133616 },
                    { 3, "Team", new DateTime(2026, 9, 17, 0, 0, 0, 0, DateTimeKind.Utc), true, "Dallas Mavericks", 134876 },
                    { 4, "Team", new DateTime(2026, 9, 17, 0, 0, 0, 0, DateTimeKind.Utc), true, "Houston Texans", 134926 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "targets",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "targets",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "targets",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "targets",
                keyColumn: "id",
                keyValue: 4);
        }
    }
}
