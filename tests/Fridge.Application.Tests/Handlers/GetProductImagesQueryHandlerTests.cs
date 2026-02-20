using FluentAssertions;
using Fridge.Application.Features.ProductImages.Queries.GetProductImages;
using Fridge.Application.Tests.Helpers;
using Fridge.Domain.Entities;

namespace Fridge.Application.Tests.Handlers;

public class GetProductImagesQueryHandlerTests
{
    [Fact]
    public async Task Should_Throw_When_Product_Not_Found()
    {
        // Given
        await using var ctx = await TestContext.CreateAsync();
        var productId = Guid.NewGuid();
        var handler = new GetProductImagesQueryHandler(ctx.Db);
        var query = new GetProductImagesQuery(productId, "BaseUrl");

        // When
        var act = () => handler.Handle(query, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Product '{productId}' not found.");
    }

    [Fact]
    public async Task Should_Get_ProductImageUrls_When_Images_Are_Exist()
    {
        // Given
        await using var ctx = await TestContext.CreateAsync();
        var productId = Guid.NewGuid();
        var productWithImage = new Product { Id = productId, Name = "A", DefaultQuantity = 10 };

        await ctx.Db.AddAndSaveAsync(productWithImage);

        var imageIdOne = Guid.NewGuid();
        var imageIdTwo = Guid.NewGuid();
        var createdAt = DateTimeOffset.UtcNow;
        await ctx.Db.AddAndSaveAsync(new ProductImage
        {
            Id = imageIdOne,
            ProductId = productWithImage.Id,
            StorageKey = "products/x.jpg",
            FileName = "firstImage.jpg",
            ContentType = "image/jpeg",
            Size = 123,
            IsPrimary = true,
            CreatedAt = createdAt
        });
        await ctx.Db.AddAndSaveAsync(new ProductImage
        {
            Id = imageIdTwo,
            ProductId = productWithImage.Id,
            StorageKey = "products/x.jpg",
            FileName = "secondImage.jpg",
            ContentType = "image/jpeg",
            Size = 123,
            IsPrimary = false,
            CreatedAt = createdAt
        });

        var handler = new GetProductImagesQueryHandler(ctx.Db);

        var baseUrl = "http://host/";
        var query = new GetProductImagesQuery(productId, baseUrl);

        // When
        var dtos = await handler.Handle(query, CancellationToken.None);

        // Then
        dtos.Should().HaveCount(2);
        dtos.Should().ContainSingle(x =>
            x.Id == imageIdOne &&
            x.FileName == "firstImage.jpg" &&
            x.ContentType == "image/jpeg" &&
            x.Size == 123 &&
            x.IsPrimary == true &&
            x.Url == $"http://host/api/products/{productId}/images/{imageIdOne}"
            );

        dtos.Should().ContainSingle(x =>
            x.Id == imageIdTwo &&
            x.FileName == "secondImage.jpg" &&
            x.ContentType == "image/jpeg" &&
            x.Size == 123 &&
            x.IsPrimary == false &&
            x.Url == $"http://host/api/products/{productId}/images/{imageIdTwo}"
            );
    }
}
