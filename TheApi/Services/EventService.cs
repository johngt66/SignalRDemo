using Microsoft.AspNetCore.SignalR;
using TheApi.Hubs;
using TheApi.Model;

namespace TheApi.Services;

public class EventService(IHubContext<TheHub, IEventClient> hubContext) : IEventService
{
    private readonly IHubContext<TheHub, IEventClient> _hubContext = hubContext;

    public Task<bool> HandleRequest(EventRequest request)
    {
        try
        {
            _hubContext.Clients.All.ProcessOutbox(request);
            return Task.FromResult(true);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error handling request: {ex.Message}");
            return Task.FromResult(false);
        }
    }

}

public interface IEventService
{
    public Task<bool> HandleRequest(EventRequest request);
}