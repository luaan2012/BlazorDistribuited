using Blazor.Api.Client.Configuration;
using Blazor.Api.Client.Endpoints;
using Blazor.Api.Client.Services;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<ClientService>();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h => {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddSwaggerConfig();

builder.Services.AddDependecyInjectionConfig();

builder.Services.AddApiConfiguration(builder.Configuration);

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseSwaggerConfig();

app.UseApiConfiguration(builder.Environment);

app.MapUserEndpoints();

app.Run();
