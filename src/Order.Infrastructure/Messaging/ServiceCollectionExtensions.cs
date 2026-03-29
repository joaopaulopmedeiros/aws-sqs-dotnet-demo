using Amazon.Runtime;
using Amazon.SQS;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

using Order.Core.Messaging;

namespace Order.Infrastructure.Messaging;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSQSProducer<TEvent>(
        this IServiceCollection services,
        string queueUrlConfigKey)
    {
        services.TryAddSQS();

        services.AddScoped<IProducer<TEvent>>(sp =>
        {
            IAmazonSQS sqsClient = sp.GetRequiredService<IAmazonSQS>();
            IConfiguration configuration = sp.GetRequiredService<IConfiguration>();
            return new SQSProducer<TEvent>(sqsClient, configuration[queueUrlConfigKey]!);
        });

        return services;
    }

    public static IServiceCollection AddSQSConsumer<TEvent, THandler>(
        this IServiceCollection services,
        string queueUrlConfigKey)
        where THandler : class, IEventHandler<TEvent>
    {
        services.TryAddSQS();

        services.AddScoped<IEventHandler<TEvent>, THandler>();

        services.AddHostedService(sp => new SQSConsumer<TEvent>(
            sp.GetRequiredService<IAmazonSQS>(),
            sp.GetRequiredService<IServiceScopeFactory>(),
            sp.GetRequiredService<IConfiguration>()[queueUrlConfigKey]!,
            sp.GetRequiredService<ILogger<SQSConsumer<TEvent>>>()
        ));

        return services;
    }

    private static void TryAddSQS(this IServiceCollection services)
    {
        services.TryAddSingleton<IAmazonSQS>(sp =>
        {
            IConfiguration configuration = sp.GetRequiredService<IConfiguration>();

            AmazonSQSConfig config = new()
            {
                ServiceURL = configuration["AWS:ServiceURL"]
            };

            return new AmazonSQSClient(
                new BasicAWSCredentials(
                    configuration["AWS:AccessKey"],
                    configuration["AWS:SecretKey"]),
                config);
        });
    }
}