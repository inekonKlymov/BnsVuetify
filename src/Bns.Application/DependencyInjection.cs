using Microsoft.Extensions.DependencyInjection;

namespace Bns.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Register API-specific services here
            // Example: services.AddControllers();

            // Register other layers' dependencies

            return services;
        }
    }
}