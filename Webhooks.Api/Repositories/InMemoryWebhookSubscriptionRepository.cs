using Webhooks.Api.Models;

namespace Webhooks.Api.Repositories;

public class InMemoryWebhookSubscriptionRepository 
{
    private List<WebhookSubscription> _subscriptions;

    public void Add(WebhookSubscription order)
    {
        _subscriptions ??= new List<WebhookSubscription>();
        _subscriptions.Add(order);
    }

    public List<WebhookSubscription> GetByEventType(string eventType)
    {
        return _subscriptions.Where(e=> e.EventType == eventType).ToList() ?? new List<WebhookSubscription>();
    }
}
