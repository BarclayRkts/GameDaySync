using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GameDay_Sync.Data;

public static class DataExtensions
{
    public static IServiceCollection AddDataLayer(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Connection string 'DefaultConnection' was not found.");

        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

        return services;
    }
}
