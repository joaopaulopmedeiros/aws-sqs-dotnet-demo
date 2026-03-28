using System.Net;
using System.Net.Http.Json;

using Microsoft.AspNetCore.Mvc.Testing;

namespace Order.WebApi.Endpoints.V1.Create.IntegrationTests;

public class CreateOrderEndpointTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();
    private const string Endpoint = "/v1/orders";

    [Fact]
    public async Task Endpoint_ValidRequest_Returns202Accepted()
    {
        CreateOrderRequest request = new(
            "customer-1",
            [
                new CreateOrderItem("product-1", 2, 9.99m)
            ]
        );

        HttpResponseMessage response = await _client.PostAsJsonAsync(Endpoint, request);

        Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
    }

    [Fact]
    public async Task Endpoint_MissingCustomerId_Returns400()
    {
        CreateOrderRequest request = new(
            string.Empty,
            [
                new CreateOrderItem("product-1", 1, 5.00m)
            ]
        );

        HttpResponseMessage response = await _client.PostAsJsonAsync(Endpoint, request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Endpoint_EmptyItems_Returns400()
    {
        CreateOrderRequest request = new(
            "customer-1",
            []
        );

        HttpResponseMessage response = await _client.PostAsJsonAsync(Endpoint, request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Endpoint_ItemWithEmptyProductId_Returns400()
    {
        CreateOrderRequest request = new(
            "customer-1",
            [
                new CreateOrderItem("", 1, 5.00m)
            ]
        );

        HttpResponseMessage response = await _client.PostAsJsonAsync(Endpoint, request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Endpoint_ItemWithZeroQuantity_Returns400()
    {
        CreateOrderRequest request = new(
            "customer-1",
            [
                new CreateOrderItem("product-1", 0, 5.00m)
            ]
        );

        HttpResponseMessage response = await _client.PostAsJsonAsync(Endpoint, request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Endpoint_ItemWithZeroUnitPrice_Returns400()
    {
        CreateOrderRequest request = new(
            "customer-1",
            [
                new CreateOrderItem("product-1", 1, 0.00m)
            ]
        );

        HttpResponseMessage response = await _client.PostAsJsonAsync(Endpoint, request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}