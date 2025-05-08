using MassTransit;
using Shared;

namespace NewsLetter.Reporting.API.Consumers;

public class ArticleCreatedConsumer:IConsumer<ArticleCreated>
{
    public Task Consume(ConsumeContext<ArticleCreated> context)
    {
        Console.WriteLine($"[Consumer] Message received: {context.Message}");
        return Task.CompletedTask;
    }
}
