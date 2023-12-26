using Microsoft.OpenApi.Models;

namespace Blazor.Api.User.Configuration;

public static class SwaggerConfig
{
    public static void AddSwaggerConfig(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo()
            {
                Title = "Identidade API",
                Contact = new OpenApiContact() { Name = "Luan Victor", Email = "oempreg.yes@outlook.com.br" },
                License = new OpenApiLicense() { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") }
            });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Insira o token JWT desta maneira: Bearer {seu token}",
                Name = "Authorization",
                Scheme = "Bearer",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }

    public static void UseSwaggerConfig(this IApplicationBuilder app, IHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }
}