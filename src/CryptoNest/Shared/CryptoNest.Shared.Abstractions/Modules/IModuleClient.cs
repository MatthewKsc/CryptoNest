using System.Threading.Tasks;

namespace CryptoNest.Shared.Abstractions.Modules;

public interface IModuleClient
{
    Task PublishAsync(object message);
}