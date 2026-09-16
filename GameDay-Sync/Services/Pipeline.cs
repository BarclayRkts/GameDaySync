using GameDay_Sync.Model;
using GameDay_Sync.Services.Extraction;
using GameDay_Sync.Services.Load;
using GameDay_Sync.Services.Notifications;

namespace GameDay_Sync.Services;

public class Pipeline(ExtractionService extractionService, LoadService loadService, NotificationsEngine notificationsEngine)
{
    public async Task RunPipeline(string alertMode)
    {
        int[] ids = [135256, 133616, 134876, 134926];
        List<SportsDbEvent> games = await extractionService.GetTeamGames(ids);
        await loadService.SaveGames(games);
        
        if (alertMode.Equals("--weekly", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Executing Weekly Notification Engine...");
            await notificationsEngine.SendWeeklyNotifications();
        }
        else if (alertMode.Equals("--daily", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Executing Daily Notification Engine...");
            await notificationsEngine.SendDailyNotifications();
        }
        else
        {
            Console.WriteLine($"Invalid mode selection: '{alertMode}'. Use '--weekly' or '--daily'.");
        }
    }
}