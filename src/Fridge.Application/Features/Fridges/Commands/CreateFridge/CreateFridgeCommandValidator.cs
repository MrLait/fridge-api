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

        When(x => x.InitialProducts is not null && x.InitialProducts.Count > 0, () =>
        {
            RuleForEach(x => x.InitialProducts!).ChildRules(item =>
            {
                item.RuleFor(i => i.ProductId).NotEmpty();
                item.RuleFor(i => i.Quantity).GreaterThan(0);
            });

            RuleFor(x => x.InitialProducts!)
                .Must(list => list.Select(i => i.ProductId).Distinct().Count() == list.Count)
                .WithMessage("InitialProducts contains duplicates ProductId.");
        });
    }
}