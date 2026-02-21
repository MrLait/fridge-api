using FluentAssertions;
using Fridge.Application.Features.Fridges.Queries.GetFridgeProducts;
using Fridge.Domain.Entities;
using Fridge.Tests.Common.Helpers;

namespace Fridge.Application.Tests.Handlers;

public class GetFridgeProductsQueryHandlerTests
{
    [Fact]
    public async Task Should_Return_Empty_List_When_No_Products_For_Fridge()
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

        var handler = new GetFridgeProductsQueryHandler(ctx.Db);
        var query = new GetFridgeProductsQuery(fridgeId);

        // When
        var dtos = await handler.Handle(query, CancellationToken.None);

        // Then
        dtos.Should().NotBeNull();
        dtos.Should().BeEmpty();
    }

    [Fact]
    public async Task Should_Return_Only_Products_For_Requested_Fridge()
    {
        // Given
        await using var ctx = await TestContext.CreateAsync();

        var modelId = Guid.NewGuid();
        await ctx.Db.AddAndSaveAsync(new FridgeModel { Id = modelId, Name = "Model", Year = 2020 });

        var fridge1 = new Domain.Entities.Fridge { Id = Guid.NewGuid(), Name = "F1", ModelId = modelId };
        var fridge2 = new Domain.Entities.Fridge { Id = Guid.NewGuid(), Name = "F2", ModelId = modelId };
        await ctx.Db.AddRangeAndSaveAsync(CancellationToken.None, fridge1, fridge2);

        var p1 = new Product { Id = Guid.NewGuid(), Name = "Milk", DefaultQuantity = 2 };
        var p2 = new Product { Id = Guid.NewGuid(), Name = "Eggs", DefaultQuantity = 10 };
        await ctx.Db.AddRangeAndSaveAsync(CancellationToken.None, p1, p2);

        var fp1 = new FridgeProduct { Id = Guid.NewGuid(), FridgeId = fridge1.Id, ProductId = p1.Id, Quantity = 3 };
        var fp2 = new FridgeProduct { Id = Guid.NewGuid(), FridgeId = fridge1.Id, ProductId = p2.Id, Quantity = 1 };
        var fpOther = new FridgeProduct { Id = Guid.NewGuid(), FridgeId = fridge2.Id, ProductId = p1.Id, Quantity = 99 };
        await ctx.Db.AddRangeAndSaveAsync(CancellationToken.None, fp1, fp2, fpOther);

        var handler = new GetFridgeProductsQueryHandler(ctx.Db);
        var query = new GetFridgeProductsQuery(fridge1.Id);

        // When
        var dtos = await handler.Handle(query, CancellationToken.None);

        // Then
        dtos.Should().HaveCount(2);
        dtos.Should().OnlyContain(x => x.ProductId == p1.Id || x.ProductId == p2.Id);
        dtos.Should().ContainSingle(x => x.Id == fp1.Id && x.ProductId == p1.Id && x.Quantity == 3);
        dtos.Should().ContainSingle(x => x.Id == fp2.Id && x.ProductId == p2.Id && x.Quantity == 1);
    }

    [Fact]
    public async Task Should_Map_ProductName_And_DefaultQuantity()
    {
        // Given
        await using var ctx = await TestContext.CreateAsync();

        var modelId = Guid.NewGuid();
        await ctx.Db.AddAndSaveAsync(new FridgeModel { Id = modelId, Name = "Model", Year = 2020 });

        var fridgeId = Guid.NewGuid();
        await ctx.Db.AddAndSaveAsync(new Domain.Entities.Fridge { Id = fridgeId, Name = "F", ModelId = modelId });

        var productId = Guid.NewGuid();
        await ctx.Db.AddAndSaveAsync(new Product { Id = productId, Name = "Cheese", DefaultQuantity = 5 });

        var fridgeProductId = Guid.NewGuid();
        await ctx.Db.AddAndSaveAsync(new FridgeProduct
        {
            Id = fridgeProductId,
            FridgeId = fridgeId,
            ProductId = productId,
            Quantity = 7
        });

        var handler = new GetFridgeProductsQueryHandler(ctx.Db);
        var query = new GetFridgeProductsQuery(fridgeId);

        // When
        var dtos = await handler.Handle(query, CancellationToken.None);

        // Then
        dtos.Should().ContainSingle(x =>
            x.Id == fridgeProductId &&
            x.ProductId == productId &&
            x.ProductName == "Cheese" &&
            x.Quantity == 7 &&
            x.ProductDefaultQuantity == 5);
    }
}