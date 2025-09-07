// See https://aka.ms/new-console-template for more information
using Microsoft.AspNetCore.SignalR.Client;
using TheService.Model;

Console.WriteLine("Hello, World!");

List<EventRequest> list = [];

// add connection to signalr hub and listen for messages
var connection = new HubConnectionBuilder()
    .WithUrl("https://localhost:7119/thehub")
    .WithAutomaticReconnect()
    .Build();
await connection.StartAsync();

connection.On("ProcessOutbox", (EventRequest request) =>
{
    list.Add(request);
});

while (true)
{
    await Task.Delay(3000);
    var item = list.FirstOrDefault();
    if (item is null)
    {
        continue;
    }
    await connection.InvokeAsync("ItemHandled", item);
    list.Remove(item);
    Console.WriteLine($"Handled message: {item.ToString()}");
}