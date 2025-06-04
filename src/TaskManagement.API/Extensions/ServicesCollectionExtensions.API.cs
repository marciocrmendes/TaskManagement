using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.IdentityModel.Tokens;
using TaskManagement.API.Configurations;
using TaskManagement.API.Configurations.Authentication;
using TaskManagement.Application;
using TaskManagement.Domain;
using TaskManagement.Infra;

namespace TaskManagement.API.Extensions;

public static class ServicesCollectionExtensionsApi
{
    public static void AddApiDependencies(this IHostApplicationBuilder builder)
    {
        var services = builder
            .Services.AddEndpointsApiExplorer()
            .AddSwaggerDependencies()
            .AddCorsDependencies()
            .AddJsonConfiguration()
            .AddJwtConfiguration(builder.Configuration)
            .AddAuthorizationPolicies()
            .AddExceptionHandler<ExceptionHandler>()
            .AddProblemDetails();

        services.AddHttpContextAccessor();
        services
            .AddApplicationDependencies()
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
            .UseExceptionHandler()
            .UseAuthentication()
            .UseAuthorization();

        app.RegisterEndpoints();
    }

    private static IServiceCollection AddCorsDependencies(this IServiceCollection services)
    {
        services.AddCors(options =>
            options.AddPolicy(
                "AllowAll",
                builder =>
                    builder
                        .SetIsOriginAllowed(_ => true)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
            )
        );

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

    private static IServiceCollection AddJwtConfiguration(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services
            .AddAuthentication(authOptions =>
            {
                authOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(bearerOptions =>
            {
                bearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["JWT:Secret"]!)
                    ),
                    ValidAudience = configuration["JWT:Issuer"],
                    ValidIssuer = configuration["JWT:Audience"],
                    ClockSkew = TimeSpan.Zero,
                };

                bearerOptions.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json; charset=utf-8";
                        var message = "An error occurred processing your authentication.";
                        return context.Response.WriteAsync(JsonSerializer.Serialize(message));
                    },
                };

                bearerOptions.SaveToken = true;
            });

        return services;
    }

    private static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
    {
        services
            .AddAuthorizationBuilder()
            .AddPolicy(
                "ManagerPolicy",
                policy =>
                {
                    policy.AddRequirements(
                        new HasScopeRequirement("taskmanagement:manager"),
                        new HasScopeRequirement("taskmanagement:user")
                    );
                }
            )
            .AddPolicy(
                "UserPolicy",
                policy => policy.AddRequirements(new HasScopeRequirement("taskmanagement:user"))
            );

        services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

        return services;
    }
}
