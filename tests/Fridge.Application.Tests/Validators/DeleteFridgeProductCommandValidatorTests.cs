using FluentAssertions;
using Fridge.Application.Features.FridgeProducts.Commands.DeleteFridgeProduct;

namespace Fridge.Application.Tests.Validators;

public class DeleteFridgeProductCommandValidatorTests
{
    [Fact]
    public void Id_Must_Be_Not_Empty()
    {
        var v = new DeleteFridgeProductCommandValidator();

        var result = v.Validate(new DeleteFridgeProductCommand(
            Guid.Empty
        ));

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Id");
    }

    [Fact]
    public void Should_Be_Valid_When_All_Fields_Are_Correct()
    {
        var v = new DeleteFridgeProductCommandValidator();

        var result = v.Validate(new DeleteFridgeProductCommand(
            Guid.NewGuid()
        ));

        result.IsValid.Should().BeTrue();
    }
}