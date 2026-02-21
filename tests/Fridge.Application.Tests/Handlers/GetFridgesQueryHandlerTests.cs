using FluentAssertions;
using Fridge.Application.Features.Fridges.Queries.GetFridges;
using Fridge.Domain.Entities;
using Fridge.Tests.Common.Helpers;

namespace Fridge.Application.Tests.Handlers;

public class GetFridgesQueryHandlerTests
{
    [Fact]
    public async Task Should_Return_Empty_List_When_No_Fridges()
    {
        // Given
        await using var ctx = await TestContext.CreateAsync();
        var handler = new GetFridgesQueryHandler(ctx.Db);
        var query = new GetFridgesQuery();

        // When
        var dtos = await handler.Handle(query, CancellationToken.None);

        // Then
        dtos.Should().NotBeNull();
        dtos.Should().BeEmpty();
    }

    [Fact]
    public async Task Should_Return_Fridges_With_Model_Info()
    {
        // Given
        await using var ctx = await TestContext.CreateAsync();

        var modelId = Guid.NewGuid();
        await ctx.Db.AddAndSaveAsync(new FridgeModel
        {
            Id = modelId,
            Name = "Model 1",
            Year = 2020
        });

        var fridgeId = Guid.NewGuid();
        await ctx.Db.AddAndSaveAsync(new Domain.Entities.Fridge
        {
            Id = fridgeId,
            Name = "Fridge 1",
            OwnerName = "Owner",
            ModelId = modelId
        });

        var handler = new GetFridgesQueryHandler(ctx.Db);
        var query = new GetFridgesQuery();

        // When
        var dtos = await handler.Handle(query, CancellationToken.None);

        // Then
        dtos.Should().HaveCount(1);

        dtos.Should().ContainSingle(x =>
            x.Id == fridgeId &&
            x.Name == "Fridge 1" &&
            x.OwnerName == "Owner" &&
            x.ModelId == modelId &&
            x.ModelName == "Model 1" &&
            x.ModelYear == 2020);
    }

    [Fact]
    public async Task Should_Return_All_Fridges()
    {
        // Given
        await using var ctx = await TestContext.CreateAsync();

        var model1 = new FridgeModel { Id = Guid.NewGuid(), Name = "Model 1", Year = 2020 };
        var model2 = new FridgeModel { Id = Guid.NewGuid(), Name = "Model 2", Year = 2021 };
        await ctx.Db.AddRangeAndSaveAsync(CancellationToken.None, model1, model2);

        var f1 = new Domain.Entities.Fridge { Id = Guid.NewGuid(), Name = "F1", OwnerName = "O1", ModelId = model1.Id };
        var f2 = new Domain.Entities.Fridge { Id = Guid.NewGuid(), Name = "F2", OwnerName = null, ModelId = model2.Id };
        await ctx.Db.AddRangeAndSaveAsync(CancellationToken.None, f1, f2);

        var handler = new GetFridgesQueryHandler(ctx.Db);

        // When
        var dtos = await handler.Handle(new GetFridgesQuery(), CancellationToken.None);

        // Then
        dtos.Should().HaveCount(2);
        dtos.Should().ContainSingle(x =>
            x.Id == f1.Id &&
            x.Name == f1.Name &&
            x.OwnerName == f1.OwnerName &&
            x.ModelId == model1.Id &&
            x.ModelName == "Model 1" &&
            x.ModelYear == 2020);

        dtos.Should().ContainSingle(x =>
            x.Id == f2.Id &&
            x.Name == f2.Name &&
            x.OwnerName == f2.OwnerName &&
            x.ModelId == model2.Id &&
            x.ModelName == "Model 2" &&
            x.ModelYear == 2021);
    }
}