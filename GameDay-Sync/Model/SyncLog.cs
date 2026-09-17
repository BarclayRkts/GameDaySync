using System.ComponentModel.DataAnnotations;

namespace GameDay_Sync.Model;

public class SyncLog
{
    [Key]
    public int Id { get; set; }

    [Required]
    public DateTime RunAt { get; set; } = DateTime.UtcNow;

    [Required]
    public string Status { get; set; } = string.Empty;

    public int GamesAdded { get; set; }

    public long DurationMs { get; set; }

    public string Message { get; set; } = string.Empty;
}
