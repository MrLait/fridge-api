using FluentValidation;

namespace Fridge.Application.Features.Fridges.Commands.UpdateFridge;

public sealed class UpdateFridgeCommandValidator
    : AbstractValidator<UpdateFridgeCommand>
{
    public UpdateFridgeCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.OwnerName).MaximumLength(200);
        RuleFor(x => x.ModelId).NotEmpty();
    }
}