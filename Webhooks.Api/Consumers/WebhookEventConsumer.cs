using MassTransit;
using Webhooks.Api.Models;
using Webhooks.Api.Services;

namespace Webhooks.Api.Consumers;

public class WebhookEventConsumer : IConsumer<WebhookEvent>
{
    private readonly ILogger<WebhookEventConsumer> _logger;
    private readonly WebhookDispatcher _webhookDispatcher;

    public WebhookEventConsumer(
        ILogger<WebhookEventConsumer> logger,
        WebhookDispatcher webhookDispatcher)
    {
        _logger = logger;
        _webhookDispatcher = webhookDispatcher;
    }

    public async Task Consume(ConsumeContext<WebhookEvent> context)
    {
        var webhookEvent = context.Message;
        
        _logger.LogInformation(
            "Received webhook event of type {EventType} with {SubscriptionCount} subscriptions",
            webhookEvent.EventType,
            webhookEvent.Subscriptions.Count);

        // Process the webhook event
        await _webhookDispatcher.DispatchAsync(webhookEvent.EventType, webhookEvent.Payload);
    }
} 