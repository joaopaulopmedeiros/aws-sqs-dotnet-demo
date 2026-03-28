using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

namespace Order.WebApi.IntegrationTests.Helpers;

public static class LocalStackHelper
{
    private const int LocalstackEdgePort = 4566;

    public static string GetServiceUrl(this IContainer localstack, int port = LocalstackEdgePort) =>
        $"http://{localstack.Hostname}:{localstack.GetMappedPublicPort(port)}";

    public static async Task<IContainer> CreateAsync(string services = "sqs,s3")
    {
        string name = $"localstack-test-{Guid.NewGuid():N}";

        IContainer localstack = new ContainerBuilder("localstack/localstack:4.13.1")
            .WithName(name)
            .WithPortBinding(LocalstackEdgePort, assignRandomHostPort: true)
            .WithEnvironment("SERVICES", services)
            .WithEnvironment("DEFAULT_REGION", "us-east-1")

            .WithWaitStrategy(
                Wait.ForUnixContainer()
                    .UntilHttpRequestIsSucceeded(req => req
                        .ForPort(LocalstackEdgePort)
                        .ForPath("/_localstack/health")
                        .ForStatusCode(System.Net.HttpStatusCode.OK)
                    )
            )
            .Build();

        await localstack.StartAsync();

        return localstack;
    }
}