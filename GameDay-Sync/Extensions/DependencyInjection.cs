using GameDay_Sync.Repository;
using GameDay_Sync.Services;
using GameDay_Sync.Services.Extraction;
using GameDay_Sync.Services.Load;
using GameDay_Sync.Services.Notifications;

namespace GameDay_Sync.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        // Register HttpClient and your SportsDataService
        services.AddHttpClient<ExtractionService>();
        services.AddScoped<LoadService>();
        services.AddScoped<GamesRepo>();
        services.AddScoped<Pipeline>();
        services.AddScoped<NotificationsEngine>();
        services.AddScoped<DiscordClient>();
        services.AddScoped<MessageFormatter>();

        return services;
    }
}