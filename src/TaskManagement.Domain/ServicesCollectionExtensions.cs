using Microsoft.Extensions.DependencyInjection;
using TaskManagement.Domain.Services;

namespace TaskManagement.Domain
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddDomainDependencies(this IServiceCollection services)
        {
            services.AddSingleton<TokenProvider>();

            return services;
        }
    }
}
