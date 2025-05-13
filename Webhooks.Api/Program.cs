using Webhooks.Api.Models;
using Webhooks.Api.Interfaces;
using Webhooks.Api.Repositories;
using Webhooks.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<InMemoryOrderRepository>();
builder.Services.AddSingleton<InMemorySubscriptionRepository>();
builder.Services.AddHttpClient<WebhookDispatcher>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/orders", (CreatedOrderRequest request, InMemoryOrderRepository orderRepository, WebhookDispatcher dispatcher) =>
{
    var order = new Order
    {
        Id = Guid.NewGuid(),
        CustomerName = request.CustomerName,
        Amount = request.Amount,
        CreatedAt = DateTime.UtcNow
    };
    orderRepository?.Add(order);

    dispatcher.DispatchAsync("order.created", order); 

    return Results.Created($"/orders/{order.Id}", order);
}).WithTags("Orders");

app.MapGet("/orders",(InMemoryOrderRepository repsitory)=>{
    var orders = repsitory.GetAll();
    return Results.Ok(orders);
}).WithTags("Orders");

app.MapPost("/webhooks/subscriptions", async (CreateSubscriptionRequest request, 
        InMemorySubscriptionRepository repository) =>
{
    var subscription = new Subscription()
    {
        Id = Guid.NewGuid(),
        Url = request.Url,
        EventType = request.EventType,
        CreatedAt = DateTime.UtcNow,
    };

    repository.Add(subscription);
    return Results.Created($"/webhooks/subscription/{subscription.Id}", subscription);
})
.WithName("CreateSubscription")
.WithOpenApi();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
