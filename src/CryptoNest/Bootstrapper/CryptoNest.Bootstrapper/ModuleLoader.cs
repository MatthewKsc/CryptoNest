using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CryptoNest.Shared.Abstractions.Modules;
using Microsoft.Extensions.Configuration;

namespace CryptoNest.Bootstrapper;

internal static class ModuleLoader
{
    public static List<Assembly> LoadAssemblies(IConfiguration configuration)
    {
        List<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
        List<string> modulesFiles = GetModulesFiles(assemblies);
        
        modulesFiles.ForEach(x => assemblies.Add(AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(x))));

        return assemblies;
    }

    public static List<IModule> LoadModules(IEnumerable<Assembly> assemblies) =>
        assemblies.SelectMany(x => x.GetTypes())
            .Where(x => typeof(IModule).IsAssignableFrom(x) && !x.IsInterface)
            .OrderBy(x => x.Name)
            .Select(Activator.CreateInstance)
            .Cast<IModule>()
            .ToList();

    private static List<string> GetModulesFiles(List<Assembly> assemblies)
    {
        string[] locations = assemblies.Where(x => !x.IsDynamic)
            .Select(x => x.Location)
            .ToArray();

        List<string> modulesFiles = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")
            .Where(x => !locations.Contains(x, StringComparer.InvariantCultureIgnoreCase))
            .ToList();

        return modulesFiles;
    }
}