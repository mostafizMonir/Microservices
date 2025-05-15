using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Webhooks.Api.Interfaces;
using Webhooks.Api.Repositories;
using Webhooks.Api.Services;
using Webhooks.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Npgsql;

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
       // services.AddSingleton<ISubscriptionRepository, InMemorySubscriptionRepository>();
        services.AddHttpClient<WebhookDispatcher>();

        return services;
    }
} 
