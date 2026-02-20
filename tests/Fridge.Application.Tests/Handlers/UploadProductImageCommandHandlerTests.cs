
using FluentAssertions;
using Fridge.Application.Common.Interfaces;
using Fridge.Application.Features.ProductImages.Commands.UploadProductImage;
using Fridge.Application.Tests.Helpers;
using Fridge.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fridge.Application.Tests.Handlers;

public class UploadProductImageCommandHandlerTests
{
    [Fact]
    public async Task Should_Throw_When_Product_Not_Found()
    {
        // Given
        await using var ctx = await TestContext.CreateAsync();
        var productId = Guid.NewGuid();
        using var content = new MemoryStream([1, 2]);

        var handler = new UploadProductImageCommandHandler(ctx.Db, new FakeFileStorage());
        var command = new UploadProductImageCommand(
            productId,
            "firstImage.jpg",
            "image/jpeg",
            123,
            content
        );

        // When
        var act = () => handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Product '{productId}' not found.");
    }

    [Fact]
    public async Task Should_Create_Primary_Image_When_First_Image()
    {
        // Given
        await using var ctx = await TestContext.CreateAsync();
        var storage = new FakeFileStorage();

        var productId = Guid.NewGuid();
        await ctx.Db.AddAndSaveAsync(new Product { Id = productId, Name = "Milk", DefaultQuantity = 2 });

        var handler = new UploadProductImageCommandHandler(ctx.Db, storage);

        using var content = new MemoryStream([1, 2, 3]);

        var command = new UploadProductImageCommand(
            productId,
            "firstImage.jpg",
            "image/jpeg",
            3,
            content
        );

        // When
        var imageId = await handler.Handle(command, CancellationToken.None);

        // Then: storage called
        storage.SaveCalls.Should().HaveCount(1);
        storage.SaveCalls[0].Path.Should().StartWith($"products/{productId}/");
        storage.SaveCalls[0].Path.Should().EndWith(".jpg");

        // Then: db row created and is primary
        var entity = await ctx.Db.ProductImages
            .SingleAsync(x => x.Id == imageId);

        entity.ProductId.Should().Be(productId);
        entity.FileName.Should().Be("firstImage.jpg");
        entity.ContentType.Should().Be("image/jpeg");
        entity.Size.Should().Be(3);
        entity.IsPrimary.Should().BeTrue();
        entity.StorageKey.Should().Be(storage.SaveCalls[0].Path);
    }

    [Fact]
    public async Task Should_Not_Set_IsPrimary_When_Primary_Already_Exists()
    {
        // Given
        await using var ctx = await TestContext.CreateAsync();
        var storage = new FakeFileStorage();

        var productId = Guid.NewGuid();
        await ctx.Db.AddAndSaveAsync(new Product { Id = productId, Name = "Milk", DefaultQuantity = 2 });

        var existingPrimaryId = Guid.NewGuid();
        await ctx.Db.AddAndSaveAsync(new ProductImage
        {
            Id = existingPrimaryId,
            ProductId = productId,
            StorageKey = $"products/{productId}/{existingPrimaryId}.jpg",
            FileName = "old.jpg",
            ContentType = "image/jpeg",
            Size = 10,
            IsPrimary = true,
            CreatedAt = DateTimeOffset.UtcNow
        });

        var handler = new UploadProductImageCommandHandler(ctx.Db, storage);

        using var content = new MemoryStream([9, 9, 9, 9]);

        var command = new UploadProductImageCommand(
            productId,
            "new.png",
            "image/png",
            4,
            content
        );

        // When
        var newImageId = await handler.Handle(command, CancellationToken.None);

        // Then
        var newEntity = await ctx.Db.ProductImages.SingleAsync(x => x.Id == newImageId);
        newEntity.IsPrimary.Should().BeFalse();

        var primaryIds = await ctx.Db.ProductImages
            .Where(x => x.ProductId == productId && x.IsPrimary)
            .Select(x => x.Id)
            .ToListAsync();

        primaryIds.Should().Equal(existingPrimaryId);

        storage.SaveCalls.Should().HaveCount(1);
        storage.SaveCalls[0].Path.Should().Contain($"{newImageId}");
        storage.SaveCalls[0].Path.Should().EndWith(".png");
    }
}
