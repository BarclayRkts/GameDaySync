using GameDay_Sync.Model;
using Microsoft.EntityFrameworkCore;

namespace GameDay_Sync.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Game> Games => Set<Game>();

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
    }
}
