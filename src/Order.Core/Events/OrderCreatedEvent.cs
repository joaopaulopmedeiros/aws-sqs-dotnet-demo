namespace Order.Core.Events;

public readonly record struct OrderCreatedEvent(
    string OrderId,
    string CustomerId,
    IReadOnlyList<OrderCreatedEventItem> Items,
    DateTimeOffset CreatedAt);

public readonly record struct OrderCreatedEventItem(
    string ProductId,
    int Quantity,
    decimal UnitPrice);