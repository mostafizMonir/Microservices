using Webhooks.Api;
using Webhooks.Api.Data;
using Webhooks.Api.Models;
using Webhooks.Api.Interfaces;
using Webhooks.Api.Repositories;
using Webhooks.Api.Services;
using Microsoft.EntityFrameworkCore;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddSingleton<InMemoryOrderRepository>();
builder.Services.AddSingleton<InMemorySubscriptionRepository>();
builder.Services.AddHttpClient<WebhookDispatcher>();

var app = builder.Build();

// Apply migrations at startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/orders", async (CreatedOrderRequest request, IRepository<Order> repository, WebhookDispatcher dispatcher, 
    IPublishEndpoint publishEndpoint, IRepository<Subscription> subscriptionRepo) =>
{
    var order = new Order
    {
        Id = Guid.NewGuid(),
        CustomerName = request.CustomerName,
        Amount = request.Amount,
        CreatedAt = DateTime.UtcNow
    };
    await repository.AddAsync(order);
    await repository.SaveChangesAsync();

    // Get subscriptions for the event
    var subscriptions = (await subscriptionRepo.GetAllAsync())
        .Where(s => s.EventType == "order.created")
        .ToList();

    // Publish webhook event to RabbitMQ
    await publishEndpoint.Publish(new WebhookEvent
    {
        EventType = "order.created",
        Payload = order,
        Subscriptions = subscriptions
    });

    return Results.Created($"/orders/{order.Id}", order);
}).WithTags("Orders");

app.MapGet("/orders",(InMemoryOrderRepository repsitory)=>{
    var orders = repsitory.GetAll();
    return Results.Ok(orders);
}).WithTags("Orders");

app.MapPost("/webhooks/subscriptions", async (CreateSubscriptionRequest request, 
        IRepository<Subscription> repository) =>
{
    var subscription = new Subscription()
    {
        Id = Guid.NewGuid(),
        Url = request.Url,
        EventType = request.EventType,
        CreatedAt = DateTime.UtcNow,
    };

   await repository.AddAsync(subscription);
    await repository.SaveChangesAsync();
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
