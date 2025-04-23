using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Infra.Context;
using TaskManagement.Infra.Context.Interceptors;
using TaskManagement.Infra.Repositories;

namespace TaskManagement.Infra
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddInfraDependencies(this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddDatabase(configuration)
                .AddRepositories();

            return services;
        }

        private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Postgres")
                            ?? throw new ArgumentException("Connection string not found");

            services.AddSingleton<PublishDomainEventsInterceptor>();

            services.AddDbContext<TaskManagementDbContext>((serviceProvider, options) => options
                .UseNpgsql(connectionString)
                .AddInterceptors(GetInterceptors(serviceProvider)));

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITaskHistoryRepository, TaskHistoryRepository>();

            return services;
        }

        private static IInterceptor[] GetInterceptors(IServiceProvider serviceProvider) => 
        [
            serviceProvider.GetRequiredService<PublishDomainEventsInterceptor>()
        ];
    }
}
