using Webhooks.Api.Models;
using Webhooks.Api.Interfaces;

namespace Webhooks.Api.Repositories;

public class InMemoryWebhookSubscriptionRepository : IRepository<Subscription>
{
    private readonly List<Subscription> _subscriptions = new();

    public Task<Subscription> AddAsync(Subscription entity)
    {
        _subscriptions.Add(entity);
        return Task.FromResult(entity);
    }

    public Task<IEnumerable<Subscription>> GetAllAsync()
    {
        return Task.FromResult(_subscriptions.AsEnumerable());
    }

    public Task<Subscription> GetByIdAsync(Guid id)
    {
        return Task.FromResult(_subscriptions.FirstOrDefault(s => s.Id == id));
    }

    public Task SaveChangesAsync()
    {
        return Task.CompletedTask;
    }
} 