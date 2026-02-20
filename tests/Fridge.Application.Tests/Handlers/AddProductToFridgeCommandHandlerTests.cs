using FluentAssertions;
using Fridge.Application.Features.Fridges.Commands.AddProductToFridge;
using Fridge.Domain.Entities;
using Fridge.Infrastructure.Services;
using Fridge.Tests.Common.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Fridge.Application.Tests.Handlers;

public class AddProductToFridgeCommandHandlerTests
{
    [Fact]
    public async Task Should_Add_Product_To_Fridge()
    {
        // Given
        await using var ctx = await TestContext.CreateAsync();

        var modelId = Guid.NewGuid();
        await ctx.Db.AddAndSaveAsync(new FridgeModel { Id = modelId, Name = "Model", Year = 2020 });

        var fridgeId = Guid.NewGuid();
        await ctx.Db.AddAndSaveAsync(new Domain.Entities.Fridge { Id = fridgeId, Name = "F", ModelId = modelId });

        var productId = Guid.NewGuid();
        await ctx.Db.AddAndSaveAsync(new Product { Id = productId, Name = "Milk", DefaultQuantity = 2 });

        var service = new FridgeProductService(ctx.Db);
        var handler = new AddProductToFridgeCommandHandler(service);

        var command = new AddProductToFridgeCommand(fridgeId, productId, 3);

        // When
        var fridgeProductId = await handler.Handle(command, CancellationToken.None);

        // Then
        var fp = await ctx.Db.FridgeProducts.SingleAsync(x => x.Id == fridgeProductId);
        fp.FridgeId.Should().Be(fridgeId);
        fp.ProductId.Should().Be(productId);
        fp.Quantity.Should().Be(3);
    }

    [Fact]
    public async Task Should_Increase_Quantity_When_Product_Already_Exists()
    {
        await using var ctx = await TestContext.CreateAsync();

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

        var service = new FridgeProductService(ctx.Db);
        var handler = new AddProductToFridgeCommandHandler(service);

        var id = await handler.Handle(new AddProductToFridgeCommand(fridgeId, productId, 2), CancellationToken.None);

        id.Should().Be(existingId);

        var fp = await ctx.Db.FridgeProducts.SingleAsync(x => x.Id == existingId);
        fp.Quantity.Should().Be(7);
    }
}