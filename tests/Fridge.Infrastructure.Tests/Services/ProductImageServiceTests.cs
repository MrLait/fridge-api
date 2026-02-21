using FluentAssertions;
using Fridge.Domain.Entities;
using Fridge.Infrastructure.Services;
using Fridge.Tests.Common.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Fridge.Infrastructure.Tests.Services;

public class ProductImageServiceTests
{
    [Fact]
    public async Task SetPrimaryAsync_Should_Throw_When_Image_Not_Found_For_Product()
    {
        // Given
        await using var ctx = await TestContext.CreateAsync();
        var service = new ProductImageService(ctx.Db);

        var productId = Guid.NewGuid();
        await ctx.Db.AddAndSaveAsync(new Product { Id = productId, Name = "Milk" });

        var missingImageId = Guid.NewGuid();

        // When
        Func<Task> act = () => service.SetPrimaryAsync(productId, missingImageId, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"*Image '{missingImageId}' not found for product '{productId}'*");
    }

    [Fact]
    public async Task SetPrimaryAsync_Should_Switch_Primary_Image()
    {
        // Given
        await using var ctx = await TestContext.CreateAsync();
        var service = new ProductImageService(ctx.Db);

        var productId = Guid.NewGuid();
        await ctx.Db.AddAndSaveAsync(new Product { Id = productId, Name = "Milk" });

        var oldPrimaryId = Guid.NewGuid();
        var newPrimaryId = Guid.NewGuid();

        await ctx.Db.AddRangeAndSaveAsync(CancellationToken.None,
            new ProductImage
            {
                Id = oldPrimaryId,
                ProductId = productId,
                StorageKey = "k1",
                FileName = "old.jpg",
                ContentType = "image/jpeg",
                Size = 1,
                IsPrimary = true,
                CreatedAt = DateTimeOffset.UtcNow
            },
            new ProductImage
            {
                Id = newPrimaryId,
                ProductId = productId,
                StorageKey = "k2",
                FileName = "new.jpg",
                ContentType = "image/jpeg",
                Size = 1,
                IsPrimary = false,
                CreatedAt = DateTimeOffset.UtcNow
            });

        // When
        await service.SetPrimaryAsync(productId, newPrimaryId, CancellationToken.None);

        // Then
        var imgs = await ctx.Db.ProductImages
            .AsNoTracking()
            .Where(x => x.ProductId == productId)
            .ToListAsync();

        imgs.Single(x => x.Id == oldPrimaryId).IsPrimary.Should().BeFalse();
        imgs.Single(x => x.Id == newPrimaryId).IsPrimary.Should().BeTrue();
        imgs.Count(x => x.IsPrimary).Should().Be(1);
    }
}
