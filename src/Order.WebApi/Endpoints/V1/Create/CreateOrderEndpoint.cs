namespace Order.WebApi.Endpoints.V1.Create;

public static class CreateOrderEndpoint
{
    public static WebApplication MapCreateOrderEndpoint(this WebApplication app)
    {
        app.MapPost("v1/orders", HandleAsync)
           .Produces(StatusCodes.Status202Accepted)
           .Produces<ValidationProblemDetails>(StatusCodes.Status400BadRequest);

        return app;
    }

    private static async Task<IResult> HandleAsync(
        CreateOrderRequest request,
        IValidator<CreateOrderRequest> validator,
        CancellationToken cancellationToken)
    {
        ValidationResult validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            IDictionary<string, string[]> errors = validationResult.ToDictionary();
            return Results.ValidationProblem(errors);
        }

        // TODO: publish to SQS

        return Results.Accepted();
    }
}