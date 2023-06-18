using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CryptoNest.Shared.Abstractions.Modules;
using Newtonsoft.Json;

namespace CryptoNest.Shared.Infrastructure.Modules;

internal sealed class ModuleClient : IModuleClient
{
    private readonly IModuleRegistry moduleRegistry;

    public ModuleClient(IModuleRegistry moduleRegistry)
    {
        this.moduleRegistry = moduleRegistry;
    }

    public async Task PublishAsync(object message)
    {
        List<Task> tasks = new();
        string key = message.GetType().Name;
        IEnumerable<ModuleBroadcastRegistration> broadcastRegistrations =
            moduleRegistry.GetModuleBroadcastRegistrations(key);

        foreach (ModuleBroadcastRegistration broadcastRegistration in broadcastRegistrations)
        {
            Func<object, Task> action = broadcastRegistration.Action;
            object receiverMessage = TranslateType(message, broadcastRegistration.ReceiverType);
            tasks.Add(action(receiverMessage));
        }

        await Task.WhenAll(tasks);
    }

    private object TranslateType(object value, Type type)
    {
        byte[] serializedBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value));

        return JsonConvert.DeserializeObject(Encoding.UTF8.GetString(serializedBytes), type);
    }
}