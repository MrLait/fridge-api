using FluentAssertions;
using Fridge.Application.Features.ProductImages.Commands.UploadProductImage;

namespace Fridge.Application.Tests.Validators;

public class UploadProductImageCommandValidatorTests
{
    [Fact]    
    public void ProductId_Must_Be_Not_Empty()
    {
        var v = new UploadProductImageCommandValidator();
        using var content = new MemoryStream([1]);

        var result = v.Validate(new UploadProductImageCommand(
            Guid.Empty,
            "FileName",
            "image/jpeg",
            10,
            content
        ));

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.PropertyName == "ProductId" &&
            e.ErrorCode == "NotEmptyValidator");
    }

    [Fact]    
    public void FileName_Must_Be_Not_Empty()
    {
        var v = new UploadProductImageCommandValidator();
        using var content = new MemoryStream([1]);

        var result = v.Validate(new UploadProductImageCommand(
            Guid.NewGuid(),
            string.Empty,
            "image/jpeg",
            10,
            content
        ));

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.PropertyName == "FileName" &&
            e.ErrorCode == "NotEmptyValidator");
    }

    [Fact]    
    public void ContentType_Must_Be_Png_or_Jpeg()
    {
        var v = new UploadProductImageCommandValidator();
        using var content = new MemoryStream([1]);

        var result = v.Validate(new UploadProductImageCommand(
            Guid.NewGuid(),
            "FileName",
            "image/type",
            10,
            content
        ));

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.PropertyName == "ContentType" &&
            e.ErrorCode == "PredicateValidator");
    }

    [Theory]    
    [InlineData("image/jpeg")]
    [InlineData("image/png")]
    public void ContentType_Should_Be_Valid(string contentType)
    {
        var v = new UploadProductImageCommandValidator();
        using var content = new MemoryStream([1]);

        var result = v.Validate(new UploadProductImageCommand(
            Guid.NewGuid(),
            "FileName",
            contentType,
            10,
            content
        ));

        result.IsValid.Should().BeTrue();
    }

    [Theory]    
    [InlineData(1)]
    [InlineData(5 * 1024 * 1024 - 1)]
    public void Size_Should_Be_Valid_When_Size_Is_Within_1b_And_5mb(long size)
    {
        var v = new UploadProductImageCommandValidator();
        using var content = new MemoryStream([1]);

        var result = v.Validate(new UploadProductImageCommand(
            Guid.NewGuid(),
            "FileName",
            "image/jpeg",
            size,
            content
        ));
        
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public void Size_Must_Be_Greater_Than_0(long size)
    {
        var v = new UploadProductImageCommandValidator();
        using var content = new MemoryStream([1]);

        var result = v.Validate(new UploadProductImageCommand(
            Guid.NewGuid(),
            "FileName",
            "image/jpeg",
            size,
            content
        ));

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.PropertyName == "Size" &&
            e.ErrorCode == "GreaterThanValidator");
    }

    [Fact]    
    public void Size_Must_Be_Less_Than_5Mb()
    {
        var v = new UploadProductImageCommandValidator();
        using var content = new MemoryStream([1]);

        var result = v.Validate(new UploadProductImageCommand(
            Guid.NewGuid(),
            "FileName",
            "image/jpeg",
            5242880,
            content
        ));

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => 
            e.PropertyName == "Size" &&
            e.ErrorCode == "LessThanValidator");
    }

    [Fact]    
    public void Should_Be_Valid_When_All_Fields_Are_Correct()
    {
        var v = new UploadProductImageCommandValidator();
        using var content = new MemoryStream([1]);

        var result = v.Validate(new UploadProductImageCommand(
            Guid.NewGuid(),
            "FileName",
            "image/jpeg",
            1,
            content
        ));

        result.IsValid.Should().BeTrue();
    }
}