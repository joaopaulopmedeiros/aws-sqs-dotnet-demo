using System.Text.Json;

using Amazon.SQS;
using Amazon.SQS.Model;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Order.Core.Messaging;

namespace Order.Infrastructure.Messaging;

public sealed class SQSConsumer<TEvent>(
    IAmazonSQS sqs,
    IServiceScopeFactory scopeFactory,
    string queueUrl,
    ILogger<SQSConsumer<TEvent>> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("SQS consumer started. Queue: {QueueUrl}", queueUrl);

        while (!stoppingToken.IsCancellationRequested)
        {
            ReceiveMessageResponse response = await sqs.ReceiveMessageAsync(new ReceiveMessageRequest
            {
                QueueUrl = queueUrl,
                MaxNumberOfMessages = 10,
                WaitTimeSeconds = 20
            }, stoppingToken);

            foreach (Message message in response.Messages)
            {
                await ProcessAsync(message, stoppingToken);
            }
        }
    }

    private async Task ProcessAsync(Message message, CancellationToken cancellationToken)
    {
        try
        {
            TEvent @event = JsonSerializer.Deserialize<TEvent>(message.Body)!;

            await using var scope = scopeFactory.CreateAsyncScope();
            IEventHandler<TEvent> handler = scope.ServiceProvider.GetRequiredService<IEventHandler<TEvent>>();

            await handler.HandleAsync(@event, cancellationToken);

            await sqs.DeleteMessageAsync(queueUrl, message.ReceiptHandle, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to process message {MessageId}", message.MessageId);
        }
    }
}