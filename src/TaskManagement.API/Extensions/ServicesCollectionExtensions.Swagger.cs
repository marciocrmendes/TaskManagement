using Microsoft.OpenApi.Models;

namespace TaskManagement.API.Extensions;

public static class ServicesCollectionExtensionsSwagger
{
    public static IServiceCollection AddSwaggerDependencies(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Task Management API",
                Version = "v1",
                Description = "API documentation for Task Management"
            });
        });

        return services;
    }

    public static WebApplication UseSwaggerDependencies(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Task Management API v1");
                options.RoutePrefix = string.Empty;
            });
        }

        return app;
    }
}