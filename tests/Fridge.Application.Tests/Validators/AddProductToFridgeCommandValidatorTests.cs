using FluentAssertions;
using Fridge.Application.Features.Fridges.Commands.AddProductToFridge;

namespace Fridge.Application.Tests.Validators;

public class AddProductToFridgeCommandValidatorTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public void Quantity_Must_Be_Greater_Than_Zero(int quantity)
    {
        var v = new AddProductToFridgeCommandValidator();

        var result = v.Validate(new AddProductToFridgeCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            quantity));

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Quantity");
    }

    [Fact]
    public void FridgeId_Must_Be_Not_Empty()
    {
        var v = new AddProductToFridgeCommandValidator();

        var result = v.Validate(new AddProductToFridgeCommand(
            Guid.Empty,
            Guid.NewGuid(),
            10));

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "FridgeId");
    }

    [Fact]
    public void ProductId_Must_Be_Not_Empty()
    {
        var v = new AddProductToFridgeCommandValidator();

        var result = v.Validate(new AddProductToFridgeCommand(
            Guid.NewGuid(),
            Guid.Empty,
            10));

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "ProductId");
    }

    [Fact]
    public void Should_Be_Valid_When_All_Fields_Are_Correct()
    {
        var v = new AddProductToFridgeCommandValidator();

        var result = v.Validate(new AddProductToFridgeCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            10));

        result.IsValid.Should().BeTrue();
    }
}
