using FluentValidation;

namespace Fridge.Application.Features.FridgeProducts.Commands.DeleteFridgeProduct;

public class DeleteFridgeProductCommandValidator : AbstractValidator<DeleteFridgeProductCommand>
{
    public DeleteFridgeProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}