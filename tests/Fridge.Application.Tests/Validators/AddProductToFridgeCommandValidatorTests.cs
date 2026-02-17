using FluentAssertions;
using Fridge.Application.Features.Fridges.Commands.AddProductToFridge;

namespace Fridge.Application.Tests.Validators;

public class AddProductToFridgeCommandValidatorTests
{
    [Fact]
    public void Quantity_Must_Be_Greater_Than_Zero()
    {
        var v = new AddProductToFridgeCommandValidator();

        var result = v.Validate(new AddProductToFridgeCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            0));

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Quantity");
    }
}
