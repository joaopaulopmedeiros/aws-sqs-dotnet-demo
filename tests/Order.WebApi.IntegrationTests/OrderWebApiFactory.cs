using DotNet.Testcontainers.Containers;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

using Order.WebApi.IntegrationTests.Helpers;

namespace Order.WebApi.IntegrationTests;

public sealed class OrderWebApiFactory
    : WebApplicationFactory<Program>, IAsyncLifetime
{
    private IContainer _container = null!;
    private string _serviceUrl = null!;
    private string _queueUrl = null!;

    async Task IAsyncLifetime.InitializeAsync()
    {
        _container = await LocalStackHelper.CreateAsync();

        _serviceUrl = _container.GetServiceUrl();

        _queueUrl = await SQSHelper.CreateQueueAsync(_serviceUrl, "orders");
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, config) => config.AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["AWS:ServiceURL"] = _serviceUrl,
            ["Messaging:QueueUrl"] = _queueUrl
        }));
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await _container.DisposeAsync();
        Dispose();
    }
}