using FluentAssertions;
using Fridge.Application.Features.Products.Queries.GetProductById;
using Fridge.Domain.Entities;
using Fridge.Tests.Common.Helpers;

namespace Fridge.Application.Tests.Handlers;
public class GetProductByIdQueryHandlerTests
{
    [Fact]
    public async Task Should_Throw_When_Product_Not_Found()
    {
        // Given
        await using var ctx = await TestContext.CreateAsync();
        var productId = Guid.NewGuid();
        var handler = new GetProductByIdQueryHandler(ctx.Db);
        var query = new GetProductByIdQuery(productId);

        // When
        var act = () => handler.Handle(query, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Product '{productId}' not found.");
    }

    [Fact]
    public async Task Should_Get_Product_By_Id()
    {
        // Given
        await using var ctx = await TestContext.CreateAsync();
        var productId = Guid.NewGuid();
        var product = new Product() { Id = productId, Name = "Name", DefaultQuantity = 10, };

        await ctx.Db.AddAndSaveAsync(product);

        var handler = new GetProductByIdQueryHandler(ctx.Db);
        var query = new GetProductByIdQuery(productId);

        // When
        var dto = await handler.Handle(query, CancellationToken.None);

        // Then
        dto.Id.Should().Be(productId);
        dto.Name.Should().Be("Name");
        dto.DefaultQuantity.Should().Be(10);
    }
}
