using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoNest.Shared.Infrastructure.Modules;

internal class ModuleRegistry : IModuleRegistry
{
    private readonly List<ModuleBroadcastRegistration> broadcastRegistrations = new();

    public IEnumerable<ModuleBroadcastRegistration> GetModuleBroadcastRegistrations(string key) =>
        broadcastRegistrations.Where(x => x.Key == key);

    public void AddBroadcastAction(Type requestType, Func<object, Task> action)
    {
        if (string.IsNullOrWhiteSpace(requestType.Namespace))
        {
            throw new InvalidOperationException("Missing namespace while adding broadcast");
        }

        ModuleBroadcastRegistration broadcastRegistration = new(requestType, action);
        broadcastRegistrations.Add(broadcastRegistration);
    }
}