using Webhooks.Api.Models;

namespace Webhooks.Api.Interfaces;

public interface IOrderRepository
{
    void Add(Order order);
    List<Order> GetAll();
} 