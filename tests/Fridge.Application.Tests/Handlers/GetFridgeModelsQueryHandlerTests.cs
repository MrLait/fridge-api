using FluentAssertions;
using Fridge.Application.Features.FridgeModels.Queries.GetFridgeModels;
using Fridge.Application.Tests.Helpers;
using Fridge.Domain.Entities;

namespace Fridge.Application.Tests.Handlers;
public class GetFridgeModelsQueryHandlerTests
{
    [Fact]
    public async Task Should_Return_Empty_List_When_No_Models()
    {
        // Given
        await using var ctx = await TestContext.CreateAsync();
        var handler = new GetFridgeModelsQueryHandler(ctx.Db);
        var query = new GetFridgeModelsQuery();

        // When
        var dtos = await handler.Handle(query, CancellationToken.None);

        // Then
        dtos.Should().BeEmpty();
    }
}