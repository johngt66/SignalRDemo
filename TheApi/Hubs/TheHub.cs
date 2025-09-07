using Microsoft.AspNetCore.SignalR;
using TheApi.Model;

namespace TheApi.Hubs;

public class TheHub : Hub<IEventClient>
{
    public Task ItemHandled(EventRequest request)
    {
        Console.WriteLine($"Item handled: {request.ToString()}");
        return Task.CompletedTask;
    }
}

public interface IEventClient
{
    Task ProcessOutbox(EventRequest request);
}
