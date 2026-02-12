using FluentValidation;

namespace Fridge.Application.Features.Fridges.Commands.AddProductToFridge;

public class AddProductToFridgeCommandValidator : AbstractValidator<AddProductToFridgeCommand>
{
    public AddProductToFridgeCommandValidator()
    {
        RuleFor(x => x.FridgeId).NotEmpty();
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.Quantity).GreaterThan(0);
    }
}