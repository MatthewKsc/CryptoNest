using System.Collections.Generic;
using System.Reflection;
using CryptoNest.Bootstrapper;
using CryptoNest.Shared.Abstractions.Modules;
using Microsoft.AspNetCore.Builder;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
WebApplication app = builder.Build();

List<Assembly> assemblies = ModuleLoader.LoadAssemblies(builder.Configuration);
List<IModule> modules = ModuleLoader.LoadModules(assemblies);

foreach (IModule module in modules)
{
    module.Register(builder.Services);
}

foreach (IModule module in modules)
{
    module.Use(app);
}


app.MapGet("/", () => "Hello World!");

app.Run();