using FluentAssertions;
using Fridge.Application.Features.Fridges.Queries.GetFridgeById;
using Fridge.Application.Tests.Helpers;
using Fridge.Domain.Entities;

namespace Fridge.Application.Tests.Handlers;
public class GetFridgeByIdQueryHandlerTests
{
    [Fact]
    public async Task Should_Throw_When_Fridge_Not_Found()
    {
        // Given
        await using var ctx = await TestContext.CreateAsync();

        var fridgeId = Guid.NewGuid();
        var handler = new GetFridgeByIdQueryHandler(ctx.Db);
        var query = new GetFridgeByIdQuery(fridgeId);

        // When
        Func<Task> act = () => handler.Handle(query, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"*Fridge '{fridgeId}' not found*");
    }

    [Fact]
    public async Task Should_Get_Fridge_By_Id_With_Model_Info()
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

        var handler = new GetFridgeByIdQueryHandler(ctx.Db);
        var query = new GetFridgeByIdQuery(fridgeId);

        // When
        var dto = await handler.Handle(query, CancellationToken.None);

        // Then
        dto.Id.Should().Be(fridgeId);
        dto.Name.Should().Be("Fridge 1");
        dto.OwnerName.Should().Be("Owner");

        dto.ModelId.Should().Be(modelId);
        dto.ModelName.Should().Be("Model 1");
        dto.ModelYear.Should().Be(2020);
    }
}