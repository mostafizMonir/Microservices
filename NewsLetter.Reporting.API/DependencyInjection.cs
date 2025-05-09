using MassTransit;
using NewsLetter.Reporting.API.Consumers;

namespace NewsLetter.Reporting.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        // Add MassTransit
        services.AddMassTransit(busConfigurator =>
            {
                busConfigurator.SetKebabCaseEndpointNameFormatter();

                 busConfigurator.AddConsumer<ArticleCreatedConsumer>();
                // busConfigurator.AddConsumer<CurrentTimeConsumerV2>();

                busConfigurator.UsingRabbitMq((context, config) =>
                {
                    config.Host(new Uri(configuration["RabbitMQ:Host"]!), h =>
                    {
                        h.Username(configuration["RabbitMQ:Username"]!);
                        h.Password(configuration["RabbitMQ:Password"]!);
                    });
                    config.ConfigureEndpoints(context);
                });
            }

        );
        return services;
    }
}
