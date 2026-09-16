using GameDay_Sync.Service;
using GameDay_Sync.Service.Extraction;

namespace GameDay_Sync.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        // Register HttpClient and your SportsDataService
        services.AddHttpClient<ExtractionService>();
        services.AddTransient<Pipeline>();

        return services;
    }
}