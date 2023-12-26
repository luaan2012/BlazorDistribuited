using Microsoft.AspNetCore.Identity;

namespace Blazor.Api.User.Configuration;

public static class IdentityPasswordPolicy
{
    public static IServiceCollection AddPasswordConfiguration(this IServiceCollection services)
    {
        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 6;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
        });

        return services;
    }
}