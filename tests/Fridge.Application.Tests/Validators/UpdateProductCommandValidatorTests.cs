using FluentAssertions;
using Fridge.Application.Features.Products.Commands.UpdateProduct;

namespace Fridge.Application.Tests.Validators;

public class UpdateProductCommandValidatorTests
{
    [Fact]    
    public void Id_Must_Be_Not_Empty()
    {
        var v = new UpdateProductCommandValidator();
        
        var result = v.Validate(new UpdateProductCommand(
            Guid.Empty,
            "Name",
            null,
            null
        ));

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.PropertyName == "Id" &&
            e.ErrorCode == "NotEmptyValidator");
    }  

    [Fact]    
    public void Name_Must_Be_Not_Empty()
    {
        var v = new UpdateProductCommandValidator();
        
        var result = v.Validate(new UpdateProductCommand(
            Guid.NewGuid(),
            string.Empty,
            null,
            null
        ));

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.PropertyName == "Name" &&
            e.ErrorCode == "NotEmptyValidator");
    }    

    
    [Fact]
    public void Name_Must_Be_Not_Longer_Than_200()
    {
        var v = new UpdateProductCommandValidator();
        var name = new string('a', 201);

        var result = v.Validate(new UpdateProductCommand(
            Guid.NewGuid(),
            name,
            null,
            null
        ));

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.PropertyName == "Name" &&
            e.ErrorCode == "MaximumLengthValidator");
    }

    [Fact]
    public void Name_Should_Be_Valid_When_Length_Is_Exactly_200()
    {
        var v = new UpdateProductCommandValidator();
        var name = new string('a', 200);

        var result = v.Validate(new UpdateProductCommand(
            Guid.NewGuid(),
            name,
            null,
            null
        ));

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public void DefaultQuantity_Must_Be_Grater_Than_0_When_Has_Value(int defaultQuantity)
    {
        var v = new UpdateProductCommandValidator();

        var result = v.Validate(new UpdateProductCommand(
            Guid.NewGuid(),
            "Name",
            defaultQuantity,
            null
        ));

        result.IsValid.Should().BeFalse();

        result.Errors.Should().Contain(e =>
            e.PropertyName == "DefaultQuantity" &&
            e.ErrorCode == "GreaterThanValidator");
    }
    
    [Fact]
    public void DefaultQuantity_Should_Be_Valid_When_Has_Positive_Value()
    {
        var v = new UpdateProductCommandValidator();

        var result = v.Validate(new UpdateProductCommand(
            Guid.NewGuid(),
            "Name",
            1,
            null
        ));

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void DefaultQuantity_Should_Be_Valid_When_Has_Is_Null()
    {
        var v = new UpdateProductCommandValidator();

        var result = v.Validate(new UpdateProductCommand(
            Guid.NewGuid(),
            "Name",
            null,
            null
        ));

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Be_Valid_When_All_Fields_Are_Correct()
    {
        var v = new UpdateProductCommandValidator();

        var result = v.Validate(new UpdateProductCommand(
            Guid.NewGuid(),
            "Name",
            1,
            "Path"
        ));

        result.IsValid.Should().BeTrue();
    }
}