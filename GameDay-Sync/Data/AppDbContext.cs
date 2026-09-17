using GameDay_Sync.Model;
using Microsoft.EntityFrameworkCore;

namespace GameDay_Sync.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Game> Games => Set<Game>();
    public DbSet<TrackedTeam> TrackedTeams => Set<TrackedTeam>();
    public DbSet<SyncLog> SyncLogs => Set<SyncLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var game = modelBuilder.Entity<Game>();

        game.ToTable("games");
        game.HasKey(x => x.Id);

        game.Property(x => x.Id)
            .HasColumnName("id")
            .HasMaxLength(50);

        game.Property(x => x.EventName)
            .HasColumnName("event_name")
            .HasMaxLength(150)
            .IsRequired();

        game.Property(x => x.TeamName)
            .HasColumnName("team_name")
            .HasMaxLength(50)
            .IsRequired();

        game.Property(x => x.OpponentName)
            .HasColumnName("opponent_name")
            .HasMaxLength(50)
            .IsRequired();

        game.Property(x => x.GameTime)
            .HasColumnName("game_time")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        game.Property(x => x.League)
            .HasColumnName("league")
            .HasMaxLength(50)
            .IsRequired();

        game.Property(x => x.WeekAlertSent)
            .HasColumnName("week_alert_sent")
            .HasDefaultValue(false);

        game.Property(x => x.DayAlertSent)
            .HasColumnName("day_alert_sent")
            .HasDefaultValue(false);

        game.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("NOW()");
        
        game.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at")
            .HasDefaultValueSql(null);

        var trackedTeam = modelBuilder.Entity<TrackedTeam>();

        trackedTeam.ToTable("tracked_teams");
        trackedTeam.HasKey(x => x.Id);

        trackedTeam.Property(x => x.Id).HasColumnName("id");

        trackedTeam.Property(x => x.SportsDbId)
            .HasColumnName("sportsdb_id")
            .IsRequired();

        trackedTeam.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(150)
            .IsRequired();

        trackedTeam.Property(x => x.Category)
            .HasColumnName("category")
            .HasMaxLength(20)
            .IsRequired();

        trackedTeam.Property(x => x.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true);

        trackedTeam.Property(x => x.DateAdded)
            .HasColumnName("date_added")
            .HasDefaultValueSql("NOW()");

        trackedTeam.HasIndex(x => x.SportsDbId).IsUnique();
        trackedTeam.HasData(
            new TrackedTeam { Id = 1, SportsDbId = 135256, Name = "Houston Astros", Category = "Team", IsActive = true, DateAdded = new DateTime(2026, 9, 17, 0, 0, 0, DateTimeKind.Utc) },
            new TrackedTeam { Id = 2, SportsDbId = 133616, Name = "Tottenham Hotspur", Category = "Team", IsActive = true, DateAdded = new DateTime(2026, 9, 17, 0, 0, 0, DateTimeKind.Utc) },
            new TrackedTeam { Id = 3, SportsDbId = 134876, Name = "Dallas Mavericks", Category = "Team", IsActive = true, DateAdded = new DateTime(2026, 9, 17, 0, 0, 0, DateTimeKind.Utc) },
            new TrackedTeam { Id = 4, SportsDbId = 134926, Name = "Houston Texans", Category = "Team", IsActive = true, DateAdded = new DateTime(2026, 9, 17, 0, 0, 0, DateTimeKind.Utc) });

        var syncLog = modelBuilder.Entity<SyncLog>();

        syncLog.ToTable("sync_logs");
        syncLog.HasKey(x => x.Id);

        syncLog.Property(x => x.Id).HasColumnName("id");

        syncLog.Property(x => x.RunAt)
            .HasColumnName("run_at")
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("NOW()");

        syncLog.Property(x => x.Status)
            .HasColumnName("status")
            .HasMaxLength(20)
            .IsRequired();

        syncLog.Property(x => x.GamesAdded)
            .HasColumnName("games_added")
            .HasDefaultValue(0);

        syncLog.Property(x => x.DurationMs)
            .HasColumnName("duration_ms")
            .HasDefaultValue(0);

        syncLog.Property(x => x.Message)
            .HasColumnName("message")
            .HasMaxLength(500);
    }
}
