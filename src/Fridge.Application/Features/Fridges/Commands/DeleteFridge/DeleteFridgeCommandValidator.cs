using FluentValidation;

namespace Fridge.Application.Features.Fridges.Commands.DeleteFridge;

public sealed class DeleteFridgeCommandValidator
    : AbstractValidator<DeleteFridgeCommand>
{
    public DeleteFridgeCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}