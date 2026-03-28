using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace Order.WebApi.IntegrationTests.Helpers;

internal static class SQSHelper
{
    internal static async Task<string> CreateQueueAsync(string serviceUrl, string queueName)
    {
        using AmazonSQSClient sqsClient = new(
            new BasicAWSCredentials("test", "test"),
            new AmazonSQSConfig { ServiceURL = serviceUrl });

        CreateQueueResponse response = await sqsClient.CreateQueueAsync(queueName);
        return response.QueueUrl;
    }
}