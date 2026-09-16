using System.ComponentModel.DataAnnotations;

namespace GameDay_Sync.Model;

public class Game
{
    [Key]
    public string Id { get; set; } = string.Empty; // Unique fixture ID from API
        
    [Required]
    public string EventName { get; set; } = string.Empty; // e.g., "Houston Rockets vs Golden State Warriors"
    
    [Required]
    public string TeamName { get; set; } = string.Empty; // e.g., "Houston Rockets"
        
    [Required]
    public string OpponentName { get; set; } = string.Empty; // e.g., "Golden State Warriors"
        
    [Required]
    public DateTime GameTime { get; set; } // Stored as UTC in Postgres
        
    [Required]
    public string League { get; set; } = string.Empty; // "NBA", "MLB", "NFL", "EPL", "MLS", "NWSL"
        
    public bool WeekAlertSent { get; set; } = false;
        
    public bool DayAlertSent { get; set; } = false;
        
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}