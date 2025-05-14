using Webhooks.Api.Models;

namespace Webhooks.Api.Repositories;

public class InMemoryWebhookSubscriptionRepository 
{
    private List<Subscription> _subscriptions;

    public void Add(Subscription order)
    {
        _subscriptions ??= new List<Subscription>();
        _subscriptions.Add(order);
    }

    public List<Subscription> GetByEventType(string eventType)
    {
        return _subscriptions.Where(e=> e.EventType == eventType).ToList() ?? new List<Subscription>();
    }
}
