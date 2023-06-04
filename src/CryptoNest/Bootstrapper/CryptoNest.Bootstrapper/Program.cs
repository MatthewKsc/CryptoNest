using CryptoNest.Bootstrapper.Modules;
using CryptoNest.Shared.Infrastructure.Modules;
using Microsoft.AspNetCore.Builder;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args)
    .ConfigureModules();

builder.RegisterModules();

WebApplication app = builder.Build();

app.UseModules(app.Logger);

app.MapGet("/", () => "Hello World!");

app.Run();