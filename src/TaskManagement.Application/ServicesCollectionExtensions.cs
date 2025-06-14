using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TaskManagement.CrossCutting.Notifications;
using TaskManagement.CrossCutting.Validation;

namespace TaskManagement.Application
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddApplicationDependencies(
            this IServiceCollection services
        )
        {
            var assembly = Assembly.GetExecutingAssembly();

            services.AddMediatR(assembly).AddFluentValidation(assembly);

            services.AddScoped<INotificationHandler, NotificationHandler>();

            return services;
        }

        private static IServiceCollection AddMediatR(
            this IServiceCollection services,
            Assembly assembly
        )
        {
            services.AddMediatR(
                delegate(MediatRServiceConfiguration config)
                {
                    config.RegisterServicesFromAssembly(assembly);
                }
            );
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }

        private static IServiceCollection AddFluentValidation(
            this IServiceCollection services,
            Assembly assembly
        )
        {
            services.AddValidatorsFromAssembly(assembly);

            return services;
        }
    }
}
