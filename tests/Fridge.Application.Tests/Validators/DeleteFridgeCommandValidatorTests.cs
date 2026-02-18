using FluentAssertions;
using Fridge.Application.Features.Fridges.Commands.DeleteFridge;

namespace Fridge.Application.Tests.Validators;

public class DeleteFridgeCommandValidatorTests
{
    [Fact]
    public void Id_Must_Be_Not_Empty()
    {
        var v = new DeleteFridgeCommandValidator();

        var result = v.Validate(new DeleteFridgeCommand(
            Guid.Empty
        ));

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => 
            e.PropertyName == "Id" &&
            e.ErrorCode == "NotEmptyValidator");
    }

    [Fact]
    public void Should_Be_Valid_When_Id_Is_Correct()
    {
        var v = new DeleteFridgeCommandValidator();

        var result = v.Validate(new DeleteFridgeCommand(Guid.NewGuid()));

        result.IsValid.Should().BeTrue();
    }
}