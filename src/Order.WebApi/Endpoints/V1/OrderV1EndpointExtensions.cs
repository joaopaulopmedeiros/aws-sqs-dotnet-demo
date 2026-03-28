namespace Order.WebApi.Endpoints.V1;

public static class OrderV1EndpointExtensions
{
    public static WebApplication MapOrderV1Endpoints(this WebApplication app)
    {
        app.MapCreateOrderEndpoint();
        return app;
    }
}