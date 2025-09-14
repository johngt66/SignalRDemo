// See https://aka.ms/new-console-template for more information
using System.Threading.Channels;
using Microsoft.AspNetCore.SignalR.Client;
using TheService.Model;

Console.WriteLine("Hello, World!");

List<EventRequest> list = [];

Channel<EventRequest> channel = Channel.CreateUnbounded<EventRequest>();
ChannelReader<EventRequest> reader = channel.Reader;
ChannelWriter<EventRequest> writer = channel.Writer;

// add connection to signalr hub and listen for messages
var connection = new HubConnectionBuilder()
    .WithUrl("https://localhost:7119/thehub")
    .WithAutomaticReconnect()
    .Build();

while (connection.State != HubConnectionState.Connected)
{
    try
    {
        await connection.StartAsync();
    }
    catch
    {
        Console.WriteLine("Connection failed, trying again in 5 seconds...");
        await Task.Delay(5000);
    }
}

connection.On("ProcessOutbox", (EventRequest request) =>
{
    Console.WriteLine($"Received message: {request.ToString()}");

    //list.Add(request);
    writer.TryWrite(request);
});

// while (true)
// {
//     Console.WriteLine($"Checking for messages... {list.Count} in queue");
//     if (list.Count == 0)
//     {
//         await Task.Delay(3000);
//         continue;
//     }

//     var item = list.First();
//     list.Remove(item);

//     Console.WriteLine($"Handling message: {item.ToString()}");

//     await Task.Delay(3000); // simulate doing some work

//     await connection.InvokeAsync("ItemHandled", item);
// }
while (true)
{
    Console.WriteLine($"Waiting for messages...");
    var item = await reader.ReadAsync();
    Console.WriteLine($"Items in queue: {reader.Count}");
    Console.WriteLine($"Processing item: {item.ToString()}");
    await Task.Delay(3000); // simulate doing some work
    await connection.InvokeAsync("ItemHandled", item);
}