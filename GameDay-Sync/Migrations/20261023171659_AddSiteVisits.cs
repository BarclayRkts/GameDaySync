using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GameDay_Sync.Migrations
{
    /// <inheritdoc />
    public partial class AddSiteVisits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "site_visits",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    visitor_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    page_path = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ip_address = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    user_agent = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    referrer = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    visited_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_site_visits", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_site_visits_visited_at",
                table: "site_visits",
                column: "visited_at");

            migrationBuilder.CreateIndex(
                name: "IX_site_visits_visitor_id",
                table: "site_visits",
                column: "visitor_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "site_visits");
        }
    }
}
