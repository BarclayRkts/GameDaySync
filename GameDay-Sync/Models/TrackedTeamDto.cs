using System.ComponentModel.DataAnnotations;

namespace GameDay_Sync.Models;

public record TrackedTeamDto(
    int Id,
    int SportsDbId,
    string Name,
    string Category,
    DateTime DateAdded,
    string Status);

public class AddTrackedTeamRequest
{
    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;
}
