using FluentAssertions;
using Fridge.Application.Features.Fridges.Commands.DeleteFridge;
using Fridge.Application.Tests.Helpers;
using Fridge.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fridge.Application.Tests.Handlers;

public class DeleteFridgeCommandHandlerTests
{
    [Fact]
    public async Task Should_Throw_When_Fridge_Not_Found()
    {
        // Given
        await using var ctx = await TestContext.CreateAsync();

        var fridgeId = Guid.NewGuid();
        var handler = new DeleteFridgeCommandHandler(ctx.Db);
        var command = new DeleteFridgeCommand(fridgeId);

        // When
        Func<Task> act = () => handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"*Fridge '{fridgeId}' not found*");
    }

    [Fact]
    public async Task Should_Delete_Fridge_When_Exists()
    {
        // Given
        await using var ctx = await TestContext.CreateAsync();

        var modelId = Guid.NewGuid();
        await ctx.Db.AddAndSaveAsync(new FridgeModel { Id = modelId, Name = "Model", Year = 2020 });

        var fridgeId = Guid.NewGuid();
        await ctx.Db.AddAndSaveAsync(new Domain.Entities.Fridge
        {
            Id = fridgeId,
            Name = "Fridge",
            OwnerName = "Owner",
            ModelId = modelId
        });

        var handler = new DeleteFridgeCommandHandler(ctx.Db);
        var command = new DeleteFridgeCommand(fridgeId);

        // When
        await handler.Handle(command, CancellationToken.None);

        // Then
        (await ctx.Db.Fridges.AnyAsync(x => x.Id == fridgeId)).Should().BeFalse();
    }

    [Fact]
    public async Task Should_Cascade_Delete_FridgeProducts_When_Deleting_Fridge()
    {
        // Given
        await using var ctx = await TestContext.CreateAsync();

        var modelId = Guid.NewGuid();
        await ctx.Db.AddAndSaveAsync(new FridgeModel { Id = modelId, Name = "Model", Year = 2020 });

        var fridgeId = Guid.NewGuid();
        await ctx.Db.AddAndSaveAsync(new Domain.Entities.Fridge
        {
            Id = fridgeId,
            Name = "Fridge",
            ModelId = modelId
        });

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

        var handler = new DeleteFridgeCommandHandler(ctx.Db);

        // When
        await handler.Handle(new DeleteFridgeCommand(fridgeId), CancellationToken.None);

        // Then
        (await ctx.Db.Fridges.AnyAsync(x => x.Id == fridgeId)).Should().BeFalse();
        (await ctx.Db.FridgeProducts.AnyAsync(x => x.Id == fridgeProductId)).Should().BeFalse();
    }
}