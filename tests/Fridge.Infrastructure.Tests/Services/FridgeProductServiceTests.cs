using FluentAssertions;
using Fridge.Domain.Entities;
using Fridge.Infrastructure.Services;
using Fridge.Tests.Common.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Fridge.Infrastructure.Tests.Services;

public class FridgeProductServiceTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public async Task AddProductAsync_Should_Throw_When_Quantity_Is_Not_Positive(int quantity)
    {
        // Given
        await using var ctx = await TestContext.CreateAsync();
        var service = new FridgeProductService(ctx.Db);

        // When
        Func<Task> act = () => service.AddProductAsync(
            fridgeId: Guid.NewGuid(),
            productId: Guid.NewGuid(),
            quantity: quantity,
            saveChanges: true,
            ct: CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<ArgumentOutOfRangeException>()
            .WithMessage("*Quantity must be grater than 0*");
    }
    [Fact]
    public async Task AddProductAsync_Should_Throw_When_Fridge_Not_Found()
    {
        // Given
        await using var ctx = await TestContext.CreateAsync();
        var service = new FridgeProductService(ctx.Db);
        var missingFridgeId = Guid.NewGuid();

        // When
        var act = () => service.AddProductAsync(
            fridgeId: missingFridgeId,
            productId: Guid.NewGuid(),
            quantity: 1,
            saveChanges: true,
            ct: CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Fridge {missingFridgeId} not found");
    }

    [Fact]
    public async Task AddProductAsync_Should_Throw_When_Product_Not_Found()
    {
        // Given
        await using var ctx = await TestContext.CreateAsync();
        var service = new FridgeProductService(ctx.Db);
        var missingProductId = Guid.NewGuid();

        var modelId = Guid.NewGuid();
        await ctx.Db.AddAndSaveAsync(new FridgeModel { Id = modelId, Name = "Model" });

        var fridgeId = Guid.NewGuid();
        await ctx.Db.AddAndSaveAsync(new Domain.Entities.Fridge { Id = fridgeId, Name = "F", ModelId = modelId });

        // When
        var act = () => service.AddProductAsync(
            fridgeId: fridgeId,
            productId: missingProductId,
            quantity: 1,
            saveChanges: true,
            ct: CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"*Product {missingProductId} not found*");
    }

    [Fact]
    public async Task AddProductAsync_Should_Create_New_FridgeProduct_When_FridgeProduct_Not_Exists()
    {
        // Given
        await using var ctx = await TestContext.CreateAsync();
        var service = new FridgeProductService(ctx.Db);

        var modelId = Guid.NewGuid();
        await ctx.Db.AddAndSaveAsync(new FridgeModel { Id = modelId, Name = "Model" });

        var fridgeId = Guid.NewGuid();
        await ctx.Db.AddAndSaveAsync(new Domain.Entities.Fridge { Id = fridgeId, Name = "F", ModelId = modelId });

        var productId = Guid.NewGuid();
        await ctx.Db.AddAndSaveAsync(new Product { Id = productId, Name = "Milk" });

        // When
        var id = await service.AddProductAsync(fridgeId, productId, quantity: 3, saveChanges: true, ct: CancellationToken.None);

        // Then
        var fp = await ctx.Db.FridgeProducts.SingleAsync(x => x.Id == id);

        fp.FridgeId.Should().Be(fridgeId);
        fp.ProductId.Should().Be(productId);
        fp.Quantity.Should().Be(3);

        (await ctx.Db.FridgeProducts.CountAsync(x => x.FridgeId == fridgeId && x.ProductId == productId))
            .Should().Be(1);
    }

    [Fact]
    public async Task AddProductAsync_Should_Increase_Quantity_When_FridgeProduct_Exists()
    {
        // Given
        await using var ctx = await TestContext.CreateAsync();
        var service = new FridgeProductService(ctx.Db);

        var modelId = Guid.NewGuid();
        await ctx.Db.AddAndSaveAsync(new FridgeModel { Id = modelId, Name = "Model" });

        var fridgeId = Guid.NewGuid();
        await ctx.Db.AddAndSaveAsync(new Domain.Entities.Fridge { Id = fridgeId, Name = "F", ModelId = modelId });

        var productId = Guid.NewGuid();
        await ctx.Db.AddAndSaveAsync(new Product { Id = productId, Name = "Milk" });

        var existingId = Guid.NewGuid();
        await ctx.Db.AddAndSaveAsync(new FridgeProduct
        {
            Id = existingId,
            FridgeId = fridgeId,
            ProductId = productId,
            Quantity = 5
        });

        // When
        var id = await service.AddProductAsync(fridgeId, productId, quantity: 2, saveChanges: true, ct: CancellationToken.None);

        // Then
        id.Should().Be(existingId);

        var fp = await ctx.Db.FridgeProducts.SingleAsync(x => x.Id == existingId);
        fp.Quantity.Should().Be(7);

        (await ctx.Db.FridgeProducts.CountAsync(x => x.FridgeId == fridgeId && x.ProductId == productId))
            .Should().Be(1);
    }
}