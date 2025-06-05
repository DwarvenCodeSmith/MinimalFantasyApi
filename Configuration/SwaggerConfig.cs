using Microsoft.OpenApi.Models;

namespace MinimalFantasyApi.Configuration;

public static class SwaggerConfig
{
    public static void AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Minimal Fantasy API",
                Version = "v1",
                Description = "A simple API for managing fantasy characters",
                Contact = new OpenApiContact
                {
                    Name = "DwarvenCodeSmith"
                }
            });
        });
    }

    public static void UseSwaggerConfiguration(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Minimal Fantasy API v1"));
    }
}