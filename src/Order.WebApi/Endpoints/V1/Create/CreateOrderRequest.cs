namespace Order.WebApi.Endpoints.V1.Create;

public readonly record struct CreateOrderRequest(
    string CustomerId,
    IReadOnlyList<CreateOrderItem> Items);

public readonly record struct CreateOrderItem(
    string ProductId,
    int Quantity,
    decimal UnitPrice);