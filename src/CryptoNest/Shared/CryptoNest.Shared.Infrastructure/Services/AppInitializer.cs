using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CryptoNest.Shared.Infrastructure.Services;

internal class AppInitializer : IHostedService
{
    private readonly IServiceProvider serviceProvider;

    public AppInitializer(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        IEnumerable<Type> dbContextTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(DbContext).IsAssignableFrom(type) && !type.IsInterface && type != typeof(DbContext));

        using IServiceScope scope = serviceProvider.CreateScope();
        
        foreach (Type dbContextType in dbContextTypes)
        {
            var dbContext = scope.ServiceProvider.GetRequiredService(dbContextType) as DbContext;
            await dbContext!.Database.MigrateAsync(cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}