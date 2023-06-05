using CryptoNest.Bootstrapper.Modules;
using CryptoNest.Shared.Infrastructure;
using CryptoNest.Shared.Infrastructure.Modules;
using Microsoft.AspNetCore.Builder;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args)
    .ConfigureModules();

builder.ConfigureInfrastructure();

builder.RegisterModules();
builder.Services.AddInfrastructure();

WebApplication app = builder.Build();

app.UseInfrastructure();
app.UseModules(app.Logger);

app.MapGet("/", () => "Hello World!");

app.Run();