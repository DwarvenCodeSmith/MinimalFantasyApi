using Microsoft.EntityFrameworkCore;
using MinimalFantasyApi.Data;
using MinimalFantasyApi.Services;

namespace MinimalFantasyApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<FantasyDbContext>(options =>
            options.UseSqlite(
                configuration.GetConnectionString("DefaultConnection") ??
                "Data Source=fantasy.db"));

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register your services
        services.AddScoped<CharacterService>();
        
        return services;
    }
}