// See https://aka.ms/new-console-template for more information
using Microsoft.AspNetCore.SignalR.Client;

Console.WriteLine("Hello, World!");

List<Guid> list = [];

// add connection to signalr hub and listen for messages
var connection = new HubConnectionBuilder()
    .WithUrl("https://localhost:7119/thehub")
    .WithAutomaticReconnect()
    .Build();
await connection.StartAsync();

connection.On("ProcessOutbox", (Guid requestId, string message) =>
{
    Console.WriteLine($"Received message: {requestId} - {message}");
    list.Add(requestId);
});

while (true)
{
    await Task.Delay(3000);
    var item = list.FirstOrDefault();
    if (item == Guid.Empty)
    {
        continue;
    }
    await connection.InvokeAsync("ItemHandled", item);
    list.Remove(item);
    Console.WriteLine($"Handled message: {item}");
}