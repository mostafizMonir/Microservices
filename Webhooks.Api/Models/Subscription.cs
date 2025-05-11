namespace Webhooks.Api.Models;

public class Subscription
{
    public Guid Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public string EventType { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
} 