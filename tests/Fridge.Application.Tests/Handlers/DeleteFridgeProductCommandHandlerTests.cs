using FluentAssertions;
using Fridge.Application.Features.FridgeProducts.Commands.DeleteFridgeProduct;
using Fridge.Domain.Entities;
using Fridge.Tests.Common.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Fridge.Application.Tests.Handlers;

public class DeleteFridgeProductCommandHandlerTests
{
    [Fact]
    public async Task Should_Throw_When_FridgeProduct_Not_Found()
    {
        // Given
        await using var ctx = await TestContext.CreateAsync();

        var id = Guid.NewGuid();
        var handler = new DeleteFridgeProductCommandHandler(ctx.Db);
        var command = new DeleteFridgeProductCommand(id);

        // When
        Func<Task> act = () => handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"*FridgeProduct '{id}' not found*");
    }

    [Fact]
    public async Task Should_Delete_FridgeProduct_When_Exists()
    {
        // Given
        await using var ctx = await TestContext.CreateAsync();

        var modelId = Guid.NewGuid();
        await ctx.Db.AddAndSaveAsync(new FridgeModel { Id = modelId, Name = "Model", Year = 2020 });

        var fridgeId = Guid.NewGuid();
        await ctx.Db.AddAndSaveAsync(new Domain.Entities.Fridge { Id = fridgeId, Name = "F", ModelId = modelId });

        var productId = Guid.NewGuid();
        await ctx.Db.AddAndSaveAsync(new Product { Id = productId, Name = "Milk", DefaultQuantity = 2 });

        var fridgeProductId = Guid.NewGuid();
        await ctx.Db.AddAndSaveAsync(new FridgeProduct
        {
            Id = fridgeProductId,
            FridgeId = fridgeId,
            ProductId = productId,
            Quantity = 3
        });

        var handler = new DeleteFridgeProductCommandHandler(ctx.Db);
        var command = new DeleteFridgeProductCommand(fridgeProductId);

        // When
        await handler.Handle(command, CancellationToken.None);

        // Then
        (await ctx.Db.FridgeProducts.AnyAsync(x => x.Id == fridgeProductId))
            .Should().BeFalse();
    }
}