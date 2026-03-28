using System.Text.Json;

using Amazon.SQS;
using Amazon.SQS.Model;

using Order.Core.Messaging;

namespace Order.Infrastructure.Messaging;

public class SQSProducer<TEvent>(IAmazonSQS sqsClient, string queueUrl) : IProducer<TEvent>
{
    public async Task ProduceAsync(TEvent @event, CancellationToken cancellationToken)
    {
        string body = JsonSerializer.Serialize(@event);

        await sqsClient.SendMessageAsync(new SendMessageRequest
        {
            QueueUrl = queueUrl,
            MessageBody = body
        }, cancellationToken);
    }
}