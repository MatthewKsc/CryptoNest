using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CryptoNest.Shared.Abstractions.Modules;
using Microsoft.Extensions.Configuration;

namespace CryptoNest.Bootstrapper.Modules;

internal static class ModuleLoader
{
    private const string ModuleNamespacePart = "CryptoNest.Modules.";
    
    public static List<Assembly> LoadAssemblies(IConfiguration configuration)
    {
        List<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
        List<string> modulesFiles = GetModulesFiles(assemblies);

        List<string> disabledModulesFiles = new();

        foreach (string moduleFile in modulesFiles)
        {
            if (!moduleFile.Contains(ModuleNamespacePart))
            {
                continue;
            }

            string moduleName = moduleFile.Split(ModuleNamespacePart)[1]
                .Split(".")[0]
                .ToLowerInvariant();

            bool isModuleEnabled = configuration.GetValue<bool>($"{moduleName}:module:enabled");

            if (!isModuleEnabled)
            {
                disabledModulesFiles.Add(moduleFile);
            }
        }
        
        disabledModulesFiles.ForEach(disabledModule => modulesFiles.Remove(disabledModule));
        
        modulesFiles.ForEach(x => assemblies.Add(AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(x))));

        return assemblies;
    }

    public static IReadOnlyCollection<IModule> LoadModules(IEnumerable<Assembly> assemblies) =>
        assemblies.SelectMany(x => x.GetTypes())
            .Where(x => typeof(IModule).IsAssignableFrom(x) && !x.IsInterface)
            .OrderBy(x => x.Name)
            .Select(Activator.CreateInstance)
            .Cast<IModule>()
            .ToArray();

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