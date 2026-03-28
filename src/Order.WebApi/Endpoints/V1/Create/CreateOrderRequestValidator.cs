namespace Order.WebApi.Endpoints.V1.Create;

public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
{
    public CreateOrderRequestValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("CustomerId is required.");

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("At least one item is required.");

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("ProductId is required.");

            item.RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");

            item.RuleFor(x => x.UnitPrice)
                .GreaterThan(0).WithMessage("UnitPrice must be greater than zero.");
        });
    }
}