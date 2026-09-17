using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameDay_Sync.Migrations
{
    public partial class RenameTargetsToTrackedTeams : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "targets",
                newName: "tracked_teams");

            migrationBuilder.RenameIndex(
                name: "IX_targets_sportsdb_id",
                table: "tracked_teams",
                newName: "IX_tracked_teams_sportsdb_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_tracked_teams_sportsdb_id",
                table: "tracked_teams",
                newName: "IX_targets_sportsdb_id");

            migrationBuilder.RenameTable(
                name: "tracked_teams",
                newName: "targets");
        }
    }
}
