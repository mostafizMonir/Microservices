using Webhooks.Api.Models;
using Webhooks.Api.Interfaces;

namespace Webhooks.Api.Repositories;

public class InMemoryOrderRepository : IOrderRepository
{
    private List<Order> _orders;

    public void Add(Order order)
    {
        _orders ??= new List<Order>();
        _orders.Add(order);
    }

    public List<Order> GetAll()
    {
        return _orders ?? new List<Order>();
    }
}
