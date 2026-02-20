using FluentAssertions;
using Fridge.Application.Features.Fridges.Commands.CreateFridge;
using Fridge.Application.Tests.Helpers;
using Fridge.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fridge.Application.Tests.Handlers;

public class CreateFridgeCommandHandlerTests
{
    [Fact]
    public async Task Should_Throw_When_Model_Not_Found()
    {
        // Given
        await using var ctx = await TestContext.CreateAsync();
        var handler = new CreateFridgeCommandHandler(ctx.Db);
        var missingModelId = Guid.NewGuid();

        var command = new CreateFridgeCommand(
            "Name",
            "OwnerName",
            missingModelId,
            []
        );

        //When
        var act = () => handler.Handle(command, CancellationToken.None);

        //Then
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"*FridgeModel '{missingModelId}' not found*");
    }

    public static IEnumerable<object?[]> EmptyInitialProductsCases()
    {
        yield return new object?[] { null };
        yield return new object?[] { Array.Empty<InitialFridgeProductItem>() };
    }

    [Theory]
    [MemberData(nameof(EmptyInitialProductsCases))]
    public async Task Should_Create_Fridge_Without_Products_When_InitialProducts_Is_Null_Or_Empty(
        IReadOnlyList<InitialFridgeProductItem>? initialProducts
    )
    {
        // Given
        await using var ctx = await TestContext.CreateAsync();

        var modelId = Guid.NewGuid();
        await ctx.Db.AddAndSaveAsync(new FridgeModel { Id = modelId, Name = "Name", Year = 2000 });

        var handler = new CreateFridgeCommandHandler(ctx.Db);
        var command = new CreateFridgeCommand("Fridge", "OwnerName", modelId, initialProducts);

        // When
        var id = await handler.Handle(command, CancellationToken.None);

        // Then
        var fridge = await LoadFridgeAsync(ctx, id);

        fridge.FridgeProducts.Should().BeEmpty();
    }

    [Fact]
    public async Task Should_Throw_When_Some_Products_Not_Found()
    {
        await using var ctx = await TestContext.CreateAsync();
        var modelId = Guid.NewGuid();
        var productOneId = Guid.NewGuid();
        var missingProductId = Guid.NewGuid();

        var initialProducts = new List<InitialFridgeProductItem>
        {
            new (productOneId, 10),
            new (missingProductId, 1)
        };

        await ctx.Db.AddAndSaveAsync(new FridgeModel { Id = modelId, Name = "Name", Year = 2000 });
        await ctx.Db.AddRangeAndSaveAsync(
            CancellationToken.None,
            new Product { Id = productOneId, Name = "ProductOne" },
            new Product { Id = Guid.NewGuid(), Name = "ProductTwo" });

        var handler = new CreateFridgeCommandHandler(ctx.Db);
        var command = new CreateFridgeCommand(
            "Fridge",
            "OwnerName",
            modelId,
            initialProducts
        );

        // When
        var act = () => handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"*Some products not found:*{missingProductId}*");

        (await ctx.Db.Fridges.CountAsync()).Should().Be(0);
    }

    [Fact]
    public async Task Should_Create_Fridge_With_Initial_Products()
    {
        await using var ctx = await TestContext.CreateAsync();
        var modelId = Guid.NewGuid();
        var productOne = new Product { Id = Guid.NewGuid(), Name = "Milk", DefaultQuantity = 2 };
        var productTwo = new Product { Id = Guid.NewGuid(), Name = "Eggs", DefaultQuantity = 10 };

        await ctx.Db.AddAndSaveAsync(new FridgeModel { Id = modelId, Name = "Name", Year = 2000 });
        await ctx.Db.AddRangeAndSaveAsync(CancellationToken.None, productOne, productTwo);

        var handler = new CreateFridgeCommandHandler(ctx.Db);
        var command = new CreateFridgeCommand(
            "Fridge",
            "OwnerName",
            modelId,
            [
                new (productOne.Id, 10),
                new (productTwo.Id, 1)
            ]
        );

        //When
        var id = await handler.Handle(command, CancellationToken.None);

        // Then
        var fridge = await LoadFridgeAsync(ctx, id);

        fridge.Name.Should().Be("Fridge");
        fridge.OwnerName.Should().Be("OwnerName");
        fridge.ModelId.Should().Be(modelId);

        fridge.FridgeProducts.Should().HaveCount(2);
        fridge.FridgeProducts.Should().ContainSingle(x => x.ProductId == productOne.Id && x.Quantity == 10);
        fridge.FridgeProducts.Should().ContainSingle(x => x.ProductId == productTwo.Id && x.Quantity == 1);
    }

    private static async Task<Domain.Entities.Fridge> LoadFridgeAsync(TestContext ctx, Guid id)
    {
        return await ctx.Db.Fridges
            .Include(x => x.FridgeProducts)
            .SingleAsync(x => x.Id == id);
    }
}