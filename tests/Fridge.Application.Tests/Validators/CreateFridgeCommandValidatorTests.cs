using FluentAssertions;
using Fridge.Application.Features.Fridges.Commands.CreateFridge;

namespace Fridge.Application.Tests.Validators;

public class CreateFridgeCommandValidatorTests
{
    [Fact]    
    public void Name_Must_Be_Not_Empty()
    {
        var v = new CreateFridgeCommandValidator();
        
        var result = v.Validate(new CreateFridgeCommand(
            string.Empty,
            "OwnerName",
            Guid.NewGuid(),
            []
        ));

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.PropertyName == "Name" &&
            e.ErrorCode == "NotEmptyValidator");
    }

    [Fact]    
    public void Name_Must_Be_Not_Longer_Than_200()
    {
        var v = new CreateFridgeCommandValidator();
        var longName = new string('a', 201);

        var result = v.Validate(new CreateFridgeCommand(
            longName,
            "OwnerName",
            Guid.NewGuid(),
            []
        ));

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => 
            e.PropertyName == "Name" &&
            e.ErrorCode == "MaximumLengthValidator");
    }

    [Fact]    
    public void OwnerName_Must_Be_Not_Longer_Than_200()
    {
        var v = new CreateFridgeCommandValidator();
        var longName = new string('a', 201);

        var result = v.Validate(new CreateFridgeCommand(
            "Name",
            longName,
            Guid.NewGuid(),
            []
        ));

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => 
            e.PropertyName == "OwnerName" &&
            e.ErrorCode == "MaximumLengthValidator");
    }

    [Fact]
    public void OwnerName_Can_Be_Null()
    {
        var v = new CreateFridgeCommandValidator();

        var result = v.Validate(new CreateFridgeCommand(
            "Name",
            null,
            Guid.NewGuid(),
            null
        ));

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void OwnerName_Can_Be_Empty_String()
    {
        var v = new CreateFridgeCommandValidator();

        var result = v.Validate(new CreateFridgeCommand(
            "Name",
            string.Empty,
            Guid.NewGuid(),
            null
        ));

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void OwnerName_Must_Be_Valid_When_Length_Is_Exactly_200()
    {
        var v = new CreateFridgeCommandValidator();
        var owner200 = new string('a', 200);

        var result = v.Validate(new CreateFridgeCommand(
            "Name",
            owner200,
            Guid.NewGuid(),
            null
        ));

        result.IsValid.Should().BeTrue();
    }

    [Fact]    
    public void ModelId_Must_Be_Not_Empty()
    {
        var v = new CreateFridgeCommandValidator();
        
        var result = v.Validate(new CreateFridgeCommand(
            "Name",
            "OwnerName",
            Guid.Empty,
            []
        ));

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => 
            e.PropertyName == "ModelId" &&
            e.ErrorCode == "NotEmptyValidator");
    }

    [Fact]
    public void ProductId_Must_Be_Not_Empty()
    {
        var v = new CreateFridgeCommandValidator();

        var initialProducts = new List<InitialFridgeProductItem>()
        {
            new (Guid.Empty, 1),
            new (Guid.NewGuid(), 1),
            new (Guid.NewGuid(), 1)
        };

        var result = v.Validate(new CreateFridgeCommand(
            "Name",
            "OwnerName",
            Guid.NewGuid(),
            initialProducts
        ));
        
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => 
            e.PropertyName.EndsWith(".ProductId") &&
            e.ErrorCode == "NotEmptyValidator");
    }

    [Fact]
    public void InitialProducts_Must_Not_Contain_Duplicates()
    {
        var v = new CreateFridgeCommandValidator();
        var productId  = Guid.NewGuid();
        var initialProducts = new List<InitialFridgeProductItem>()
        {
            new (productId, 1),
            new (productId, 1),
            new (Guid.NewGuid(), 1)
        };

        var result = v.Validate(new CreateFridgeCommand(
            "Name",
            "OwnerName",
            Guid.NewGuid(),
            initialProducts
        ));

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => 
            e.PropertyName == "InitialProducts" &&
            e.ErrorCode == "PredicateValidator");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public void Quantity_Must_Be_Greater_Than_Zero(int quantity)
    {
        var v = new CreateFridgeCommandValidator();
        var initialProducts = new List<InitialFridgeProductItem>()
        {
            new (Guid.NewGuid(), 1),
            new (Guid.NewGuid(), quantity),
            new (Guid.NewGuid(), 1)
        };

        var result = v.Validate(new CreateFridgeCommand(
            "Name",
            "OwnerName",
            Guid.NewGuid(),
            initialProducts
        ));

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => 
            e.PropertyName.EndsWith(".Quantity") &&
            e.ErrorCode == "GreaterThanValidator");
    }

    [Fact]
    public void Should_Be_Valid_When_InitialProducts_Is_Null()
    {
        var v = new CreateFridgeCommandValidator();

        var result = v.Validate(new CreateFridgeCommand(
            "Name",
            "OwnerName",
            Guid.NewGuid(),
            null
        ));

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Be_Valid_When_InitialProducts_Is_Empty_List()
    {
        var v = new CreateFridgeCommandValidator();

        var result = v.Validate(new CreateFridgeCommand(
            "Name",
            "OwnerName",
            Guid.NewGuid(),
            []
        ));

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Be_Valid_When_All_Fields_Are_Correct()
    {
        var v = new CreateFridgeCommandValidator();
        var initialProducts = new List<InitialFridgeProductItem>()
        {
            new (Guid.NewGuid(), 1),
            new (Guid.NewGuid(), 1),
            new (Guid.NewGuid(), 1)
        };

        var result = v.Validate(new CreateFridgeCommand(
            "Name",
            "OwnerName",
            Guid.NewGuid(),
            initialProducts
        ));

        result.IsValid.Should().BeTrue();
    }
}