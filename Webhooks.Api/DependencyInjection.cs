using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Webhooks.Api.Interfaces;
using Webhooks.Api.Repositories;
using Webhooks.Api.Services;
using Webhooks.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Npgsql;
using MassTransit;
using Webhooks.Api.Consumers;

namespace Webhooks.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register DbContext with PostgreSQL
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        // Register Generic Repository
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        // Register other services
        services.AddSingleton<InMemoryOrderRepository>();
        services.AddSingleton<InMemoryWebhookSubscriptionRepository>();
        services.AddHttpClient<WebhookDispatcher>();

        // Add MassTransit with RabbitMQ
        services.AddMassTransit(x =>
        {
            // Register consumers
            x.AddConsumer<WebhookEventConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                // Configure the consumer
                cfg.ReceiveEndpoint("webhook-events", e =>
                {
                    e.ConfigureConsumer<WebhookEventConsumer>(context);
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
} 
