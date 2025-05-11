using Webhooks.Api.Models;

namespace Webhooks.Api.Interfaces;

public interface ISubscriptionRepository
{
    void Add(Subscription subscription);
    List<Subscription> GetAll();
    Subscription? GetById(Guid id);
    void Update(Subscription subscription);
    void Delete(Guid id);
} 