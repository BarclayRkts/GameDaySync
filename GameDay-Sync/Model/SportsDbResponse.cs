using System.Text.Json.Serialization;

namespace GameDay_Sync.Model;

public class SportsDbResponse
{
    [JsonPropertyName("events")]
    public List<SportsDbEvent>? Events { get; set; }
}

public class SportsDbTeamSearchResponse
{
    [JsonPropertyName("teams")]
    public List<SportsDbTeam>? Teams { get; set; }
}

public class SportsDbTeam
{
    [JsonPropertyName("idTeam")]
    public string IdTeam { get; set; } = string.Empty;

    [JsonPropertyName("strTeam")]
    public string Name { get; set; } = string.Empty;
}

// Mirrors the individual event fields inside the raw JSON payload
public class SportsDbEvent
{
    [JsonPropertyName("idEvent")]
    public string IdEvent { get; set; } = string.Empty;

    [JsonPropertyName("strEvent")]
    public string StrEvent { get; set; } = string.Empty;

    [JsonPropertyName("strHomeTeam")]
    public string StrHomeTeam { get; set; } = string.Empty;

    [JsonPropertyName("strAwayTeam")]
    public string StrAwayTeam { get; set; } = string.Empty;

    [JsonPropertyName("strLeague")]
    public string StrLeague { get; set; } = string.Empty;

    [JsonPropertyName("strTimestamp")]
    public string StrTimestamp { get; set; } = string.Empty;
}