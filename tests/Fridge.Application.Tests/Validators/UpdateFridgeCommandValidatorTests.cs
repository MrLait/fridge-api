using FluentAssertions;
using Fridge.Application.Features.Fridges.Commands.UpdateFridge;

namespace Fridge.Application.Tests.Validators;

public class UpdateFridgeCommandValidatorTests
{
    [Fact]    
    public void Id_Must_Be_Not_Empty()
    {
        var v = new UpdateFridgeCommandValidator();
        
        var result = v.Validate(new UpdateFridgeCommand(
            Guid.Empty,
            "Name",
            "OwnerName",
            Guid.NewGuid()
        ));

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.PropertyName == "Id" &&
            e.ErrorCode == "NotEmptyValidator");
    }

    [Fact]    
    public void Name_Must_Be_Not_Empty()
    {
        var v = new UpdateFridgeCommandValidator();
        
        var result = v.Validate(new UpdateFridgeCommand(
            Guid.NewGuid(),
            string.Empty,
            "OwnerName",
            Guid.NewGuid()
        ));

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.PropertyName == "Name" &&
            e.ErrorCode == "NotEmptyValidator");
    }

    [Fact]    
    public void ModelId_Must_Be_Not_Empty()
    {
        var v = new UpdateFridgeCommandValidator();
        
        var result = v.Validate(new UpdateFridgeCommand(
            Guid.NewGuid(),
            "Name",
            "OwnerName",
            Guid.Empty
        ));

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.PropertyName == "ModelId" &&
            e.ErrorCode == "NotEmptyValidator");
    }

    [Fact]
    public void Name_Must_Be_Not_Longer_Than_200()
    {
        var v = new UpdateFridgeCommandValidator();
        var longString = new string('a', 201);

        var result = v.Validate(new UpdateFridgeCommand(
            Guid.NewGuid(),
            longString,
            "OwnerName",
            Guid.NewGuid()
        ));

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.PropertyName == "Name" &&
            e.ErrorCode == "MaximumLengthValidator");
    }

    [Fact]
    public void OwnerName_Must_Be_Not_Longer_Than_200()
    {
        var v = new UpdateFridgeCommandValidator();
        var longString = new string('a', 201);
        var result = v.Validate(new UpdateFridgeCommand(
            Guid.NewGuid(),
            "longString",
            longString,
            Guid.NewGuid()
        ));

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.PropertyName == "OwnerName" &&
            e.ErrorCode == "MaximumLengthValidator");
    }

    [Fact]
    public void OwnerName_Can_Be_Null()
    {
        var v = new UpdateFridgeCommandValidator();

        var result = v.Validate(new UpdateFridgeCommand(
            Guid.NewGuid(),
            "Name",
            null,
            Guid.NewGuid()
        ));

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void OwnerName_Can_Be_Empty_String()
    { 
        var v = new UpdateFridgeCommandValidator();

        var result = v.Validate(new UpdateFridgeCommand(
            Guid.NewGuid(),
            "Name",
            string.Empty,
            Guid.NewGuid()
        ));

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(200)]
    public void Name_Should_Be_Valid_When_Length_Is_Within_1_And_200(int length)
    {
        var v = new UpdateFridgeCommandValidator();
        var name = new string('a', length);

        var result = v.Validate(new UpdateFridgeCommand(
            Guid.NewGuid(),
            name,
            "OwnerName",
            Guid.NewGuid()
        ));

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(200)]
    public void OwnerName_Should_Be_Valid_When_Length_Is_Within_1_And_200(int length)
    {
        var v = new UpdateFridgeCommandValidator();
        var ownerName = new string('a', length);

        var result = v.Validate(new UpdateFridgeCommand(
            Guid.NewGuid(),
            "longString",
            ownerName,
            Guid.NewGuid()
        ));

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Be_Valid_When_All_Fields_Are_Correct()
    {
        var v = new UpdateFridgeCommandValidator();

            var result = v.Validate(new UpdateFridgeCommand(
            Guid.NewGuid(),
            "Name",
            "OwnerName",
            Guid.NewGuid()
        ));

         result.IsValid.Should().BeTrue();
    }
}