using System.Net.Http.Json;
using Webhooks.Api.Models;
using Webhooks.Api.Interfaces;
using Webhooks.Api.Repositories;

namespace Webhooks.Api.Services;

public class WebhookDispatcher
{
    private readonly HttpClient _httpClient;
    private readonly IRepository<Subscription> _subscriptionRepository;
    private readonly ILogger<WebhookDispatcher> _logger;

    public WebhookDispatcher(
        HttpClient httpClient,
        IRepository<Subscription> subscriptionRepository,
        ILogger<WebhookDispatcher> logger)
    {
        _httpClient = httpClient;
        _subscriptionRepository = subscriptionRepository;
        _logger = logger;
    }

    public async Task DispatchAsync(string eventType, object payload)
    {
        var subscriptions = (await _subscriptionRepository.GetAllAsync())
            .Where(s => s.EventType == eventType && s.IsActive)
            .ToList();

        foreach (var subscription in subscriptions)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(subscription.Url, payload);
                
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning(
                        "Failed to deliver webhook to {Url} for event {EventType}. Status code: {StatusCode}",
                        subscription.Url,
                        eventType,
                        response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error delivering webhook to {Url} for event {EventType}",
                    subscription.Url,
                    eventType);
            }
        }
    }
} 
