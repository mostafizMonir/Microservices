using Microsoft.AspNetCore.Mvc;
using Webhooks.Api.Interfaces;
using Webhooks.Api.Models;
using Webhooks.Api.Services;

namespace Webhooks.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WebhookController : ControllerBase
{
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly WebhookDispatcher _webhookDispatcher;
    private readonly ILogger<WebhookController> _logger;

    public WebhookController(
        ISubscriptionRepository subscriptionRepository,
        WebhookDispatcher webhookDispatcher,
        ILogger<WebhookController> logger)
    {
        _subscriptionRepository = subscriptionRepository;
        _webhookDispatcher = webhookDispatcher;
        _logger = logger;
    }

    [HttpPost("subscribe")]
    public async Task<IActionResult> Subscribe([FromBody] WebhookSubscription subscription)
    {
        try
        {
            var result = await _subscriptionRepository.AddSubscriptionAsync(subscription);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding webhook subscription");
            return StatusCode(500, "Error adding webhook subscription");
        }
    }

    [HttpPost("unsubscribe/{id}")]
    public async Task<IActionResult> Unsubscribe(string id)
    {
        try
        {
            var result = await _subscriptionRepository.RemoveSubscriptionAsync(id);
            if (result)
            {
                return Ok();
            }
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing webhook subscription");
            return StatusCode(500, "Error removing webhook subscription");
        }
    }

    [HttpGet("subscriptions")]
    public async Task<IActionResult> GetSubscriptions()
    {
        try
        {
            var subscriptions = await _subscriptionRepository.GetAllSubscriptionsAsync();
            return Ok(subscriptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting webhook subscriptions");
            return StatusCode(500, "Error getting webhook subscriptions");
        }
    }

    [HttpPost("notify")]
    public async Task<IActionResult> Notify([FromBody] WebhookEvent webhookEvent)
    {
        try
        {
            await _webhookDispatcher.DispatchEventAsync(webhookEvent);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error dispatching webhook event");
            return StatusCode(500, "Error dispatching webhook event");
        }
    }
} 