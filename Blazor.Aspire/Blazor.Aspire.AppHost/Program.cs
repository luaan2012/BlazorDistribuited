var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedisContainer("rediscache");

var apiUser = builder.AddProject<Projects.Blazor_Api_User>("blazor.api.user");

builder.AddProject<Projects.Blazor_Api_Client>("blazor.api.client");

builder.AddProject<Projects.Blazor_Web>("blazor.web")
    .WithReference(apiUser)
    .WithReference(cache);

builder.Build().Run();