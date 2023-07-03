using System;
using System.Collections.Generic;
using System.Reflection;
using CryptoNest.Bootstrapper.Modules;
using CryptoNest.Shared.Infrastructure;
using CryptoNest.Shared.Infrastructure.Modules;
using Microsoft.AspNetCore.Builder;
using NLog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args)
    .ConfigureModules();

builder.ConfigureInfrastructure();

List<Assembly> assemblies = ModuleLoader.LoadAssemblies(builder.Configuration);

builder.RegisterModules(assemblies);
builder.Services.AddInfrastructure(assemblies);

try
{
    WebApplication app = builder.Build();

    app.UseInfrastructure();
    app.UseModules(app.Logger);

    app.MapControllers();
    app.MapGet("/", () => "Crypto Nest API!");

    app.Run();
}
catch (Exception exception)
{
    Logger logger = NLog.LogManager.GetCurrentClassLogger();
    logger.Error(exception, "Exception occured while starting application");
}
finally
{
    NLog.LogManager.Shutdown();
}