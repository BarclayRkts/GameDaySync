using System.Text;
using System.Text.Json;
using GameDay_Sync.Model;
using Microsoft.Extensions.Configuration;

namespace GameDay_Sync.Services.Notifications;

public class DiscordClient(HttpClient httpClient, IConfiguration configuration)
{
    public async Task<bool> SendToDiscordAsync(NotificationPayload payload)
    {
        string? webhookUrl = configuration["NotificationSettings:DiscordWebhookUrl"];
        if (string.IsNullOrEmpty(webhookUrl))
        {
            Console.WriteLine("Error: Discord Webhook URL is missing from appsettings.json configuration.");
            return false;
        }
        
        try
        {
            string jsonBody = JsonSerializer.Serialize(payload);
            var httpContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            Console.WriteLine("Sending bundled notification package to Discord...");
            var response = await httpClient.PostAsync(webhookUrl, httpContent);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Successfully delivered bundled alerts to Discord.");
                return true;
            }
            
            string errorReason = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Failed to route webhook. Status: {response.StatusCode}. Reason: {errorReason}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Network error occurred while connecting to Discord: {ex.Message}");
            return false;
        }
    }
}
