using Microsoft.AspNetCore.Http.Json;
using System.Text.Json.Serialization;
using TaskManagement.API.Configurations;
using TaskManagement.Domain;
using TaskManagement.Infra;

namespace TaskManagement.API.Extensions;

public static class ServicesCollectionExtensionsApi
{
    public static void AddApiDependencies(this IHostApplicationBuilder builder)
    {
        var services = builder.Services
            .AddEndpointsApiExplorer()
            .AddSwaggerDependencies()
            .AddCorsDependencies()
            .AddJsonConfiguration()
            .AddExceptionHandler<ExceptionHandler>()
            .AddProblemDetails();

        services
            .AddDomainDependencies()
            .AddInfraDependencies(builder.Configuration);
    }

    public static void UseApiDependencies(this WebApplication app)
    {
        app.UseSwaggerDependencies()
           .UseCors("AllowAll")
           .UsePathBase("/task-management")
           .UseForwardedHeaders()
           .UseRouting()
           .UseExceptionHandler();

        app.RegisterEndpoints();
    }

    private static IServiceCollection AddCorsDependencies(this IServiceCollection services)
    {
        services
            .AddCors(options => options
            .AddPolicy("AllowAll", builder => builder
                .SetIsOriginAllowed(_ => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()));

        return services;
    }

    private static IServiceCollection AddJsonConfiguration(this IServiceCollection services)
    {
        services.Configure<JsonOptions>(options =>
        {
            options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });

        return services;
    }
}