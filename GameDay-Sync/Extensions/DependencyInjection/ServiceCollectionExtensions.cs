using GameDay_Sync.Repositories;
using GameDay_Sync.Services;

namespace GameDay_Sync.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAdminApi(this IServiceCollection services)
    {
        services.AddScoped<IGamesRepository, GamesRepository>();
        services.AddScoped<ITrackedTeamsRepository, TrackedTeamsRepository>();
        services.AddScoped<ISyncLogsRepository, SyncLogsRepository>();

        services.AddScoped<IGamesQueryService, GamesQueryService>();
        services.AddScoped<ITrackedTeamsService, TrackedTeamsService>();
        services.AddScoped<ISyncLogsService, SyncLogsService>();
        services.AddScoped<IDashboardService, DashboardService>();

        return services;
    }
}
