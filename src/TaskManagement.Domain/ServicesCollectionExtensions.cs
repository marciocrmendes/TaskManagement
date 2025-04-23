using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TaskManagement.CrossCutting.Notifications;
using TaskManagement.CrossCutting.Validation;
using TaskManagement.Domain.Services;

namespace TaskManagement.Domain
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddDomainDependencies(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            services
                .AddMediatR(assembly)
                .AddFluentValidation(assembly);

            services.AddScoped<INotificationHandler, NotificationHandler>();

            services.AddSingleton<TokenProvider>();

            return services;
        }

        private static IServiceCollection AddMediatR(this IServiceCollection services, Assembly assembly)
        {
            services.AddMediatR(delegate (MediatRServiceConfiguration config)
            {
                config.RegisterServicesFromAssembly(assembly);
            });
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }

        private static IServiceCollection AddFluentValidation(this IServiceCollection services, Assembly assembly)
        {
            services.AddValidatorsFromAssembly(assembly);

            return services;
        }
    }
}
