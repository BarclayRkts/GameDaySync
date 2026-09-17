namespace GameDay_Sync.Models;

public record SyncLogDto(
    int Id,
    DateTime RunAt,
    string Status,
    int GamesAdded,
    long DurationMs,
    string Message);
