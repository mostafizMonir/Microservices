namespace Webhooks.Api.Models;

public class CreateSubscriptionRequest
{
    public string Url { get; set; } = string.Empty;
    public string EventType { get; set; } = string.Empty;
} 