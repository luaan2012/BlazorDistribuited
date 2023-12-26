using Blazor.Api.User.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Blazor.Api.User.Configuration;

public static class IdentityConfig
{

    public static void AddIdentityConfig(this IServiceCollection services, IConfiguration configuration)
    {
        var connection = configuration.GetConnectionString("ConnectionString");
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("ConnectionString")));

        services.AddIdentity<IdentityUser, IdentityRole>()
            .AddErrorDescriber<IdentityMessageConfig>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddPasswordConfiguration();
    }
}