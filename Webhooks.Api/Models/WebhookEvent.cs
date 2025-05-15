using Webhooks.Api.Models;

namespace Webhooks.Api.Models;

public class WebhookEvent
{
    public string EventType { get; set; }
    public object Payload { get; set; }
    public List<Subscription> Subscriptions { get; set; }
} 