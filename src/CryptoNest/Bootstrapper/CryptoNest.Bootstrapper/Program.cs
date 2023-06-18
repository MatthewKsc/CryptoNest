using System.Collections.Generic;
using System.Reflection;
using CryptoNest.Bootstrapper.Modules;
using CryptoNest.Shared.Infrastructure;
using CryptoNest.Shared.Infrastructure.Modules;
using Microsoft.AspNetCore.Builder;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args)
    .ConfigureModules();

builder.ConfigureInfrastructure();

List<Assembly> assemblies = ModuleLoader.LoadAssemblies(builder.Configuration);

builder.RegisterModules(assemblies);
builder.Services.AddInfrastructure(assemblies);

WebApplication app = builder.Build();

app.UseInfrastructure();
app.UseModules(app.Logger);

app.MapGet("/", () => "Hello World!");

app.Run();