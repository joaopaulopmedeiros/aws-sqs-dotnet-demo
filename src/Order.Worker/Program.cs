var builder = Host.CreateApplicationBuilder(args);

builder.AddTelemetry("order-worker");

builder.Services.AddSQSConsumer<OrderCreatedEvent, OrderCreatedEventHandler>("Messaging:QueueUrl");

await builder.Build().RunAsync();