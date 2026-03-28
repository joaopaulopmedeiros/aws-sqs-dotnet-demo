var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddScoped<IValidator<CreateOrderRequest>, CreateOrderRequestValidator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapOrderV1Endpoints();

app.Run();