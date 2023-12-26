using Blazor.Api.Client.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using NetDevPack.Security.JwtExtensions;

namespace Blazor.Api.Client.Configuration
{
    public static class ApiConfig
    {
        public static void AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEndpointsApiExplorer();

            var connection = configuration.GetConnectionString("ConnectionString");

            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(configuration.GetConnectionString("ConnectionString")));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(jwt =>
                {
                    jwt.RequireHttpsMetadata = true;
                    jwt.SaveToken = true;
                    jwt.SetJwksOptions(new JwkOptions("https://localhost:7078/jwks"));
                });

            services.AddAuthorization();
        }

        public static void UseApiConfiguration(this IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();
        }
    }
}
