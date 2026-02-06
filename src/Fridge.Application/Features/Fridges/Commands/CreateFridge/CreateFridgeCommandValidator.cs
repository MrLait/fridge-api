using FluentValidation;

namespace Fridge.Application.Features.Fridges.Commands.CreateFridge;

public sealed class CreateFridgeCommandValidator
    : AbstractValidator<CreateFridgeCommand>
{
    public CreateFridgeCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.OwnerName).MaximumLength(200);
        RuleFor(x => x.ModelId).NotEmpty();
    }
}