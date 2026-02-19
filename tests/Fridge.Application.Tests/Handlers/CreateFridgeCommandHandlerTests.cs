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
        var (db, conn) = await TestDbContextFactory.CreateAsync();
        await using var _ = conn;
        var handler = new CreateFridgeCommandHandler(db);
        var missingModelId = Guid.NewGuid();

        var command = new CreateFridgeCommand(
            "Name",
            "OwnerName",
            missingModelId,
            []
        );

        //When
        var act = async () => await handler.Handle(command, CancellationToken.None);

        //Then
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"*FridgeModel '{missingModelId}' not found*");
    }

    [Fact]
    public async Task Should_Create_Fridge_Without_Products_When_InitialProducts_Is_Empty()
    {
        var (db, conn) = await TestDbContextFactory.CreateAsync();
        await using var _ = conn;

        var modelId = Guid.NewGuid();
        db.FridgeModels.Add(new FridgeModel { Id = modelId, Name = "Name", Year = 2000 });
        await db.SaveChangesAsync();

        var handler = new CreateFridgeCommandHandler(db);
        var command = new CreateFridgeCommand(
            "Fridge",
            "OwnerName",
            modelId,
            []
        );

        //When
        var id = await handler.Handle(command, CancellationToken.None);

        // Then
        var fridge = await db.Fridges
           .Include(x => x.FridgeProducts)
           .SingleOrDefaultAsync(x => x.Id == id);

        fridge!.Name.Should().Be("Fridge");
        fridge.OwnerName.Should().Be("OwnerName");
        fridge.ModelId.Should().Be(modelId);
        fridge.FridgeProducts.Should().BeEmpty();
    }

    [Fact]
    public async Task Should_Create_Fridge_Without_Products_When_InitialProducts_Is_Null()
    {
        var (db, conn) = await TestDbContextFactory.CreateAsync();
        await using var _ = conn;

        var modelId = Guid.NewGuid();
        db.FridgeModels.Add(new FridgeModel { Id = modelId, Name = "Name", Year = 2000 });
        await db.SaveChangesAsync();

        var handler = new CreateFridgeCommandHandler(db);
        var command = new CreateFridgeCommand(
            "Fridge",
            "OwnerName",
            modelId,
            null
        );

        //When
        var id = await handler.Handle(command, CancellationToken.None);

        // Then
        var fridge = await db.Fridges
           .Include(x => x.FridgeProducts)
           .SingleOrDefaultAsync(x => x.Id == id);

        fridge!.Name.Should().Be("Fridge");
        fridge.OwnerName.Should().Be("OwnerName");
        fridge.ModelId.Should().Be(modelId);
        fridge.FridgeProducts.Should().BeEmpty();
    }

    [Fact]
    public async Task Should_Throw_When_Some_Products_Not_Found()
    {
        var (db, conn) = await TestDbContextFactory.CreateAsync();
        await using var _ = conn;
        var modelId = Guid.NewGuid();
        var productOneId = Guid.NewGuid();
        var missingProductId = Guid.NewGuid();

        var initialProducts = new List<InitialFridgeProductItem>
        {
            new (productOneId, 10),
            new (missingProductId, 1)
        };

        db.FridgeModels.Add(new FridgeModel { Id = modelId, Name = "Name", Year = 2000 });
        db.FridgeModels.Add(new FridgeModel { Id = modelId, Name = "Name", Year = 2000 });

        db.Products.Add(new Product { Id = productOneId, Name = "ProductOne" });
        db.Products.Add(new Product { Id = Guid.NewGuid(), Name = "ProductTwo" });
        await db.SaveChangesAsync();

        var handler = new CreateFridgeCommandHandler(db);
        var command = new CreateFridgeCommand(
            "Fridge",
            "OwnerName",
            modelId,
            initialProducts
        );

        // When
        var act = async () => await handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"*Some products not found:*{missingProductId}*");

        (await db.Fridges.CountAsync()).Should().Be(0);
    }

    [Fact]
    public async Task Should_Create_Fridge_With_Initial_Products()
    {
        var (db, conn) = await TestDbContextFactory.CreateAsync();
        await using var _ = conn;
        var modelId = Guid.NewGuid();

        db.FridgeModels.Add(new FridgeModel { Id = modelId, Name = "Name", Year = 2000 });

        var productOne = new Product { Id = Guid.NewGuid(), Name = "Milk", DefaultQuantity = 2 };
        var productTwo = new Product { Id = Guid.NewGuid(), Name = "Eggs", DefaultQuantity = 10 };
        db.Products.AddRange(productOne, productTwo);
        await db.SaveChangesAsync();

        var handler = new CreateFridgeCommandHandler(db);
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
        var fridge = await db.Fridges
           .Include(x => x.FridgeProducts)
           .SingleAsync(x => x.Id == id);

        fridge!.Name.Should().Be("Fridge");
        fridge.OwnerName.Should().Be("OwnerName");
        fridge.ModelId.Should().Be(modelId);

        fridge.FridgeProducts.Should().HaveCount(2);
        fridge.FridgeProducts.Should().ContainSingle(x => x.ProductId == productOne.Id && x.Quantity == 10);
        fridge.FridgeProducts.Should().ContainSingle(x => x.ProductId == productTwo.Id && x.Quantity == 1);
    }
}