using Blazor.Api.User.Helper;
using Blazor.Api.User.Services;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Blazor.Api.User.Configuration;

public static class DependecyInjectionConfig
{
    public static void AddDependecyInjectionConfig(this IServiceCollection services)
    {
        services.AddTransient<IEmailSender, Emailsender>();
        services.AddScoped<IAuthServices, AuthServices>();
    }
}