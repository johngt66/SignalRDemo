using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TheApi.Model;
using TheApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSignalR();
builder.Services.AddSingleton<IEventService, EventService>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapHub<TheApi.Hubs.TheHub>("/thehub");

app.MapPost("/SendEvent", async Task<Results<Ok, BadRequest>> (IEventService eventService, [FromBody] EventRequest request) =>
    await eventService.HandleRequest(request)
        ? TypedResults.Ok()
        : TypedResults.BadRequest())
    .WithName("SendEvent")
    .WithOpenApi();

await app.RunAsync();

