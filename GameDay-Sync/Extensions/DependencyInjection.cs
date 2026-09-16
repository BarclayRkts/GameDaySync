using GameDay_Sync.Services;
using GameDay_Sync.Services.Extraction;
using GameDay_Sync.Services.Load;

namespace GameDay_Sync.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        // Register HttpClient and your SportsDataService
        services.AddHttpClient<ExtractionService>();
        services.AddScoped<LoadService>();
        services.AddScoped<Pipeline>();

        return services;
    }
}