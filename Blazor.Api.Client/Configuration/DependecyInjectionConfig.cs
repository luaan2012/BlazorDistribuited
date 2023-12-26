using Blazor.Api.Client.Repository;
using Blazor.Api.Client.Services;

namespace Blazor.Api.Client.Configuration
{
    public static class DependecyInjectionConfig
    {
        public static void AddDependecyInjectionConfig(this IServiceCollection services)
        {
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IClientService, ClientService>();
        }
    }
}
