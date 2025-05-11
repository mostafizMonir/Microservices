namespace Webhooks.Api.Models;

public record Order
{
    public Guid Id { get; set; }
    public string? CustomerName { get; set; }
   
    public decimal Amount { get; set; }
   
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public record CreatedOrderRequest
{
    public string? CustomerName { get; set; }

    public decimal Amount { get; set; }

}
