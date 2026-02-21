using FluentAssertions;
using Fridge.Application.Features.ProductImages.Commands.DeleteProductImage;
using Fridge.Application.Tests.Helpers;
using Fridge.Domain.Entities;
using Fridge.Tests.Common.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Fridge.Application.Tests.Handlers;
public class DeleteProductImageCommandHandlerTests
{
    [Fact]
    public async Task Should_Throw_When_Image_Not_Found()
    {
        // Given
        await using var ctx = await TestContext.CreateAsync();
        var storage = new FakeFileStorage();

        var handler = new DeleteProductImageCommandHandler(ctx.Db, storage);

        var productId = Guid.NewGuid();
        var imageId = Guid.NewGuid();

        var command = new DeleteProductImageCommand(productId, imageId);

        // When
        var act = () => handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"*Image '{imageId}' not found for product '{productId}'*");
    }

    [Fact]
    public async Task Should_Delete_Image_From_Storage_And_Db()
    {
        // Given
        await using var ctx = await TestContext.CreateAsync();
        var storage = new FakeFileStorage();

        var productId = Guid.NewGuid();
        await ctx.Db.AddAndSaveAsync(new Product { Id = productId, Name = "Milk" });

        var imageId = Guid.NewGuid();
        var storageKey = $"products/{productId}/{imageId}.jpg";

        await ctx.Db.AddAndSaveAsync(new ProductImage
        {
            Id = imageId,
            ProductId = productId,
            StorageKey = storageKey,
            FileName = "x.jpg",
            ContentType = "image/jpeg",
            Size = 123,
            IsPrimary = true,
            CreatedAt = DateTimeOffset.UtcNow
        });

        var handler = new DeleteProductImageCommandHandler(ctx.Db, storage);
        var command = new DeleteProductImageCommand(productId, imageId);

        // When
        await handler.Handle(command, CancellationToken.None);

        // Then: storage
        storage.DeleteCalls.Should().ContainSingle(storageKey);

        // Then: db row removed
        var exists = await ctx.Db.ProductImages.AnyAsync(x => x.Id == imageId);
        exists.Should().BeFalse();
    }

}