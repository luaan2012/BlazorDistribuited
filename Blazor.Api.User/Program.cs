using Blazor.Api.User.Configuration;
using Blazor.Api.User.Endpoints;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h => {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ConfigureEndpoints(context);
    });
});

builder.AddServiceDefaults();

builder.Services.AddApiConfiguration(builder.Configuration);

builder.Services.AddDependecyInjectionConfig();

builder.Services.AddIdentityConfig(builder.Configuration);

builder.Services.AddSwaggerConfig();

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseSwaggerConfig(builder.Environment);

//if (app.Environment.IsDevelopment())
//{
//    IdentityModelEventSource.ShowPII = true;

//    using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
//    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//    await db.Database.EnsureCreatedAsync();
//}

app.UseApiConfiguration();

app.MapUserEndpoints();

app.Run();
