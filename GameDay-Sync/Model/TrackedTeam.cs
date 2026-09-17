using System.ComponentModel.DataAnnotations;

namespace GameDay_Sync.Model;

public class TrackedTeam
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int SportsDbId { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Category { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public DateTime DateAdded { get; set; } = DateTime.UtcNow;
}
