using FluentAssertions;
using Fridge.Application.Features.Products.Commands.UpdateProduct;
using Fridge.Application.Tests.Helpers;
using Fridge.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fridge.Application.Tests.Handlers;
public class UpdateProductCommandHandlerTests
{
    [Fact]
    public async Task Should_Throw_When_Product_Not_Found()
    {
        // Given
        await using var ctx = await TestContext.CreateAsync();
        var productId = Guid.NewGuid();
        var handler = new UpdateProductCommandHandler(ctx.Db);
        var command = new UpdateProductCommand(productId, "Name", null, null);

        // When
        var act = () => handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Product '{productId}' not found.");
    }

    [Fact]
    public async Task Should_Update_Product()
    {
        // Given
        await using var ctx = await TestContext.CreateAsync();
        var productId = Guid.NewGuid();
        var product = new Product() { Id = productId, Name = "Name", DefaultQuantity = 10, };

        await ctx.Db.AddAndSaveAsync(product);

        var handler = new UpdateProductCommandHandler(ctx.Db);
        var command = new UpdateProductCommand(productId, "NewName", 1, null);

        // When
        var dto = await handler.Handle(command, CancellationToken.None);
        var entity = await ctx.Db.Products
            .SingleAsync(x => x.Id == productId);

        // Then
        dto.Id.Should().Be(productId);
        dto.Name.Should().Be("NewName");
        dto.DefaultQuantity.Should().Be(1);

        entity.Name.Should().Be("NewName");
        entity.DefaultQuantity.Should().Be(1);
    }

    [Fact]
    public async Task Should_Set_DefaultQuantity_To_Null_When_Command_DefaultQuantity_Is_Null()
    {
        // Given
        await using var ctx = await TestContext.CreateAsync();

        var productId = Guid.NewGuid();
        await ctx.Db.AddAndSaveAsync(new Product
        {
            Id = productId,
            Name = "Name",
            DefaultQuantity = 10
        });

        var handler = new UpdateProductCommandHandler(ctx.Db);
        var command = new UpdateProductCommand(productId, "NewName", null, null);

        // When
        var dto = await handler.Handle(command, CancellationToken.None);
        var entity = await ctx.Db.Products.SingleAsync(x => x.Id == productId);

        // Then (DTO)
        dto.Id.Should().Be(productId);
        dto.Name.Should().Be("NewName");
        dto.DefaultQuantity.Should().BeNull();

        // Then (DB)
        entity.Name.Should().Be("NewName");
        entity.DefaultQuantity.Should().BeNull();
    }
}