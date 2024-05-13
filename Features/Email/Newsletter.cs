using MassTransit;

namespace BlazorServerMassTransit.Features.Email;

public record SubscribeToNewsletter(string EmailAddress);

public record NewsletterSubscriptionConfirmed(Guid SubscriberId, string EmailAddress);

public record NewsletterSubscriptionDenied(string EmailAddress);

public class SubscribeToNewsletterConsumer : IConsumer<SubscribeToNewsletter>
{
    public async Task Consume(ConsumeContext<SubscribeToNewsletter> context)
    {
        await Task.Delay(2000);
        
        var emailAddress = context.Message.EmailAddress;
        if (string.IsNullOrEmpty(emailAddress) || emailAddress.Contains("test"))
        {
            await context.Publish(new NewsletterSubscriptionDenied(emailAddress));
            return;
        }
        await context.Publish(new NewsletterSubscriptionConfirmed(Guid.NewGuid(), emailAddress));
    }
}

public class NewsletterSubscriptionConfirmedHandler(ILogger<NewsletterSubscriptionConfirmedHandler> logger) : IConsumer<NewsletterSubscriptionConfirmed>
{
    public Task Consume(ConsumeContext<NewsletterSubscriptionConfirmed> context)
    {
        logger.LogInformation("Newsletter subscription confirmed");
        return Task.CompletedTask;
    }
}

public class NewsletterSubscriptionDeniedHandler(ILogger<NewsletterSubscriptionDeniedHandler> logger) : IConsumer<NewsletterSubscriptionDenied>
{
    public Task Consume(ConsumeContext<NewsletterSubscriptionDenied> context)
    {
        logger.LogInformation("Newsletter subscription denied");
        return Task.CompletedTask;
    }
}