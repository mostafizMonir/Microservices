using Webhooks.Api.Models;
using Webhooks.Api.Interfaces;

namespace Webhooks.Api.Repositories;

public class InMemorySubscriptionRepository : ISubscriptionRepository
{
    private List<Subscription> _subscriptions;

    public void Add(Subscription subscription)
    {
        _subscriptions ??= new List<Subscription>();
        _subscriptions.Add(subscription);
    }

    public List<Subscription> GetAll()
    {
        return _subscriptions ?? new List<Subscription>();
    }

    public Subscription? GetById(Guid id)
    {
        return _subscriptions?.FirstOrDefault(s => s.Id == id);
    }

    public void Update(Subscription subscription)
    {
        if (_subscriptions == null) return;
        
        var index = _subscriptions.FindIndex(s => s.Id == subscription.Id);
        if (index != -1)
        {
            _subscriptions[index] = subscription;
        }
    }

    public void Delete(Guid id)
    {
        if (_subscriptions == null) return;
        
        var subscription = _subscriptions.FirstOrDefault(s => s.Id == id);
        if (subscription != null)
        {
            _subscriptions.Remove(subscription);
        }
    }
} 