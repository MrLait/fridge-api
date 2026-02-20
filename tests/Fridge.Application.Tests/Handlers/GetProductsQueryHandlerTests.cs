using FluentAssertions;
using Fridge.Application.Features.Products.Queries.GetProducts;
using Fridge.Domain.Entities;
using Fridge.Tests.Common.Helpers;

namespace Fridge.Application.Tests.Handlers;
public class GetProductsQueryHandlerTests
{
    [Fact]
    public async Task Should_Get_Products()
    {
        // Given
        await using var ctx = await TestContext.CreateAsync();
        var (productOneId, productTwoId) = (Guid.NewGuid(), Guid.NewGuid());
        var productOne = new Product() { Id = productOneId, Name = "NameOne", DefaultQuantity = 1, };
        var productTwo = new Product() { Id = productTwoId, Name = "NameTwo", DefaultQuantity = 2, };

        await ctx.Db.AddRangeAndSaveAsync(CancellationToken.None, [productOne, productTwo]);

        var handler = new GetProductsQueryHandler(ctx.Db);
        var query = new GetProductsQuery("BaseUrl");

        // When
        var dtos = await handler.Handle(query, CancellationToken.None);

        // Then
        dtos.Should().HaveCount(2);

        dtos.Should().ContainSingle(x => x.Id == productOneId && x.Name == "NameOne" && x.DefaultQuantity == 1 && x.PrimaryImageUrl == null);
        dtos.Should().ContainSingle(x => x.Id == productTwoId && x.Name == "NameTwo" && x.DefaultQuantity == 2 && x.PrimaryImageUrl == null);
    }

    [Fact]
    public async Task Should_Set_PrimaryImageUrl_When_Primary_Image_Exists()
    {
        // Given
        await using var ctx = await TestContext.CreateAsync();

        var productWithImage = new Product { Id = Guid.NewGuid(), Name = "A", DefaultQuantity = 10 };
        var productWithoutImage = new Product { Id = Guid.NewGuid(), Name = "B", DefaultQuantity = 20 };

        await ctx.Db.AddRangeAndSaveAsync(CancellationToken.None, productWithImage, productWithoutImage);

        var imageId = Guid.NewGuid();
        await ctx.Db.AddAndSaveAsync(new ProductImage
        {
            Id = imageId,
            ProductId = productWithImage.Id,
            StorageKey = "products/x.jpg",
            FileName = "x.jpg",
            ContentType = "image/jpeg",
            Size = 123,
            IsPrimary = true,
            CreatedAt = DateTimeOffset.UtcNow
        });

        var handler = new GetProductsQueryHandler(ctx.Db);

        var baseUrl = "http://host/";
        var query = new GetProductsQuery(baseUrl);

        // When
        var dtos = await handler.Handle(query, CancellationToken.None);

        // Then
        var dtoWithImage = dtos.Single(x => x.Id == productWithImage.Id);
        dtoWithImage.PrimaryImageUrl.Should()
            .Be($"http://host/api/products/{productWithImage.Id}/images/{imageId}");

        var dtoWithoutImage = dtos.Single(x => x.Id == productWithoutImage.Id);
        dtoWithoutImage.PrimaryImageUrl.Should().BeNull();
    }
}
