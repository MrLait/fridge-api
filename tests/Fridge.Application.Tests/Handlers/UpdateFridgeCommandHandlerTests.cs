using FluentAssertions;
using Fridge.Application.Features.Fridges.Commands.UpdateFridge;
using Fridge.Application.Tests.Helpers;
using Fridge.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fridge.Application.Tests.Handlers;

public class UpdateFridgeCommandHandlerTests
{
    [Fact]
    public async Task Should_Throw_When_Fridge_Not_Found()
    {
        // Given
        await using var ctx = await TestContext.CreateAsync();

        var handler = new UpdateFridgeCommandHandler(ctx.Db);
        var id = Guid.NewGuid();

        var command = new UpdateFridgeCommand(
            id,
            "NewName",
            "NewOwner",
            Guid.NewGuid());

        // When
        Func<Task> act = () => handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"*Fridge '{id}' not found*");
    }

    [Fact]
    public async Task Should_Throw_When_Model_Changed_And_New_Model_Not_Found()
    {
        // Given
        await using var ctx = await TestContext.CreateAsync();

        var existingModelId = Guid.NewGuid();
        await ctx.Db.AddAndSaveAsync(new FridgeModel { Id = existingModelId, Name = "M1", Year = 2020 });

        var fridgeId = Guid.NewGuid();
        await ctx.Db.AddAndSaveAsync(new Domain.Entities.Fridge
        {
            Id = fridgeId,
            Name = "Fridge",
            OwnerName = "Owner",
            ModelId = existingModelId
        });

        var missingModelId = Guid.NewGuid();
        var handler = new UpdateFridgeCommandHandler(ctx.Db);

        var command = new UpdateFridgeCommand(fridgeId, "NewName", "NewOwner", missingModelId);

        // When
        Func<Task> act = () => handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"*FridgeModel '{missingModelId}' not found*");

        // and fridge not updated
        var fridge = await ctx.Db.Fridges.SingleAsync(x => x.Id == fridgeId);
        fridge.Name.Should().Be("Fridge");
        fridge.OwnerName.Should().Be("Owner");
        fridge.ModelId.Should().Be(existingModelId);
    }

    [Fact]
    public async Task Should_Update_Fridge_When_Model_Not_Changed()
    {
        // Given
        await using var ctx = await TestContext.CreateAsync();

        var modelId = Guid.NewGuid();
        await ctx.Db.AddAndSaveAsync(new FridgeModel { Id = modelId, Name = "M1", Year = 2020 });

        var fridgeId = Guid.NewGuid();
        await ctx.Db.AddAndSaveAsync(new Domain.Entities.Fridge
        {
            Id = fridgeId,
            Name = "Fridge",
            OwnerName = "Owner",
            ModelId = modelId
        });

        var handler = new UpdateFridgeCommandHandler(ctx.Db);
        var command = new UpdateFridgeCommand(fridgeId, "NewName", null, modelId);

        // When
        await handler.Handle(command, CancellationToken.None);

        // Then
        var fridge = await ctx.Db.Fridges.SingleAsync(x => x.Id == fridgeId);
        fridge.Name.Should().Be("NewName");
        fridge.OwnerName.Should().BeNull();
        fridge.ModelId.Should().Be(modelId);
    }

    [Fact]
    public async Task Should_Update_Fridge_When_Model_Changed_To_Existing_Model()
    {
        // Given
        await using var ctx = await TestContext.CreateAsync();

        var model1 = new FridgeModel { Id = Guid.NewGuid(), Name = "M1", Year = 2020 };
        var model2 = new FridgeModel { Id = Guid.NewGuid(), Name = "M2", Year = 2021 };
        await ctx.Db.AddRangeAndSaveAsync(CancellationToken.None, model1, model2);

        var fridgeId = Guid.NewGuid();
        await ctx.Db.AddAndSaveAsync(new Domain.Entities.Fridge
        {
            Id = fridgeId,
            Name = "Fridge",
            OwnerName = "Owner",
            ModelId = model1.Id
        });

        var handler = new UpdateFridgeCommandHandler(ctx.Db);
        var command = new UpdateFridgeCommand(fridgeId, "NewName", "NewOwner", model2.Id);

        // When
        await handler.Handle(command, CancellationToken.None);

        // Then
        var fridge = await ctx.Db.Fridges.SingleAsync(x => x.Id == fridgeId);
        fridge.Name.Should().Be("NewName");
        fridge.OwnerName.Should().Be("NewOwner");
        fridge.ModelId.Should().Be(model2.Id);
    }
}