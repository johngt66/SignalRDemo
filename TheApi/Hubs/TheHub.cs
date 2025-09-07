using Microsoft.AspNetCore.SignalR;
using TheApi.Model;

namespace TheApi.Hubs;

public class TheHub : Hub<IEventClient>
{
    public Task ItemHandled(Guid requestId)
    {
        Console.WriteLine($"Item handled: {requestId}");
        return Task.CompletedTask;
    }
}

public interface IEventClient
{
    Task ProcessOutbox(EventRequest request);
}
