using Microsoft.Extensions.DependencyInjection;

namespace Bns.Domain
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            // Register API-specific services here
            // Example: services.AddControllers();

            // Register other layers' dependencies

            return services;
        }
    }
}