using GameDay_Sync.Model;
using GameDay_Sync.Repository;

namespace GameDay_Sync.Services.Notifications;

public class NotificationsEngine(GamesRepo gamesRepo, DiscordClient discordClient, MessageFormatter formatter)
{
    public async Task SendWeeklyNotifications(bool includeAlreadyAlerted = false)
    {
        var gamesThisWeek = await gamesRepo.GetWeeklyGames(includeAlreadyAlerted);
        if (gamesThisWeek.Count == 0)
        {
            Console.WriteLine("No new games scheduled for this week.");
        }

        string messageBody = formatter.BuildWeeklyMessage(gamesThisWeek);
        var payload = new NotificationPayload { Content = messageBody };
        
        bool success = await discordClient.SendToDiscordAsync(payload);
        if (success)
        {
            foreach (var game in gamesThisWeek)
            {
                game.WeekAlertSent = true;
            }
            
            await gamesRepo.SaveTrackingChanges();
            Console.WriteLine("Weekly notification batch complete.");
        }
    }
    
    public async Task SendDailyNotifications(bool includeAlreadyAlerted = false)
    {
        var gamesToday = await gamesRepo.GetDailyGames(includeAlreadyAlerted);
        if (gamesToday.Count == 0)
        {
            Console.WriteLine("No games scheduled for today.");
        }
        
        string messageBody = formatter.BuildDailyMessage(gamesToday);
        var payload = new NotificationPayload { Content = messageBody };

        bool success = await discordClient.SendToDiscordAsync(payload);
        if (success)
        {
            foreach (var game in gamesToday)
            {
                game.DayAlertSent = true;
            }

            await gamesRepo.SaveTrackingChanges();
            Console.WriteLine("Daily notification batch complete.");
        }
    }
    
}