using System.Text;
using GameDay_Sync.Model;

namespace GameDay_Sync.Services.Notifications;

public class MessageFormatter
{
    public string BuildWeeklyMessage(List<Game> games)
    {
        var sb = new StringBuilder();
        sb.AppendLine("📅 🚀 **WEEKLY SPORTS SCHEDULE UPDATE** 🚀 📅");
        sb.AppendLine("Here are the upcoming games involving your target teams:");
        sb.AppendLine("---------------------------------------------------------");

        foreach (var game in games)
        {
            DateTime ctTime = ConvertUtcToCentralTime(game.GameTime);
            string formattedDate = ctTime.ToString("dddd, MMMM dd");
            string formattedTime = ctTime.ToString("h:mm tt");
            string emoji = GetSportEmoji(game.League);

            sb.AppendLine($"{emoji} **{game.TeamName}** vs **{game.OpponentName}**");
            sb.AppendLine($"📅 Date: {formattedDate}");
            sb.AppendLine($"⏰ Time: {formattedTime} CT");
            sb.AppendLine("---------------------------------------------------------");
        }

        return sb.ToString();
    }

    public string BuildDailyMessage(List<Game> games)
    {
        var sb = new StringBuilder();
        sb.AppendLine("🚨 🔥 📢 **IT'S GAME DAY! GET HYPED!** 📢 🔥 🚨");
        sb.AppendLine("Your teams are in action today! Catch the schedules below:");
        sb.AppendLine("=========================================================");

        foreach (var game in games)
        {
            DateTime ctTime = ConvertUtcToCentralTime(game.GameTime);
            string formattedTime = ctTime.ToString("h:mm tt");
            string emoji = GetSportEmoji(game.League);

            sb.AppendLine($"{emoji} **{game.TeamName}** takes on **{game.OpponentName}** TODAY!");
            sb.AppendLine($"⏰ Kickoff/Tipoff: **{formattedTime} CT**");
            sb.AppendLine("=========================================================");
        }

        return sb.ToString();
    }
    
    private DateTime ConvertUtcToCentralTime(DateTime utcDateTime)
    {
        TimeZoneInfo centralZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
        return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, centralZone);
    }

    private string GetSportEmoji(string league)
    {
        if (league.Equals("NFL", StringComparison.OrdinalIgnoreCase)) return "🏈";
        if (league.Equals("NBA", StringComparison.OrdinalIgnoreCase)) return "🚀";
        if (league.Equals("MLB", StringComparison.OrdinalIgnoreCase)) return "⚾";
        return "⚽";
    }
}
