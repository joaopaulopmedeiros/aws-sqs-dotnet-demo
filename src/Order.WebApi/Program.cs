using OpenTelemetry.Metrics;

var builder = WebApplication.CreateSlimBuilder(args);

builder.AddTelemetry("order-api",
    configureMetrics: m => m.AddAspNetCoreInstrumentation());

builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();
builder.Services.AddScoped<IValidator<CreateOrderRequest>, CreateOrderRequestValidator>();
builder.Services.AddSQSProducer<OrderCreatedEvent>("Messaging:QueueUrl");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapHealthChecks("/health");

app.MapOrderV1Endpoints();

app.Run();