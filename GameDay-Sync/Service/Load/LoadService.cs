using System.Globalization;
using GameDay_Sync.Data;
using GameDay_Sync.Model;

namespace GameDay_Sync.Service.Load;

public class LoadService(AppDbContext appDbContext)
{
    public async Task SaveGames(List<SportsDbEvent> games)
    {
        foreach (var sportEvent in games)
        {
            DateTime timeStampToUtcDateTime = DateTime.Parse(sportEvent.StrTimestamp, CultureInfo.InvariantCulture,
                DateTimeStyles.AdjustToUniversal);

            var doesGamesExist = await appDbContext.Games.FindAsync(sportEvent.IdEvent);

            if (doesGamesExist == null)
            {
                var newEvent = new Game()
                {
                    Id = sportEvent.IdEvent,
                    EventName = sportEvent.StrEvent,
                    TeamName = sportEvent.StrHomeTeam,
                    OpponentName = sportEvent.StrAwayTeam,
                    GameTime = DateTime.SpecifyKind(timeStampToUtcDateTime, DateTimeKind.Utc),
                    League = sportEvent.StrLeague
                };

                appDbContext.Games.Add(newEvent);
            }

        }
        
        await appDbContext.SaveChangesAsync(); 
    }
}