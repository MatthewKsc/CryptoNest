using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoNest.Shared.Infrastructure.Modules;

public interface IModuleRegistry
{
    IEnumerable<ModuleBroadcastRegistration> GetModuleBroadcastRegistrations(string key);
    void AddBroadcastAction(Type requestType, Func<object, Task> action);
}