using FluentAssertions;
using Fridge.Application.Common.Exceptions;
using Fridge.Domain.Entities;
using Fridge.Infrastructure.Services;
using Fridge.Infrastructure.Tests.Helpers;
using Fridge.Tests.Common.Helpers;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Fridge.Infrastructure.Tests.Services;

[Collection("SqlServer")]
public class RestockZeroQuantityAsyncTests(SqlServerContainerFixture fx)
{
    [Fact]
    public async Task RestockZeroQuantityAsync_Should_Return_0_When_No_Candidates()
    {
        await using var ctx = await SqlServerTestContext.CreateAsync(fx.ConnectionString);

        await CreateStoredProcedureAsync(ctx.Db.Database.GetConnectionString()!);

        var fridgeProductService = new FridgeProductService(ctx.Db);
        var service = new RestockService(ctx.Db, fridgeProductService);

        var updated = await service.RestockZeroQuantityAsync(CancellationToken.None);

        updated.Should().Be(0);
    }

    [Fact]
    public async Task RestockZeroQuantityAsync_Should_Throw_When_DefaultQuantity_Is_Invalid()
    {
        // Given
        await using var ctx = await SqlServerTestContext.CreateAsync(fx.ConnectionString);
        await CreateStoredProcedureAsync(ctx.Db.Database.GetConnectionString()!);

        var modelId = Guid.NewGuid();
        var fridgeId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var fridgeProductId = Guid.NewGuid();

        await ctx.Db.AddRangeAndSaveAsync(CancellationToken.None,
            new FridgeModel { Id = modelId, Name = "M" },
            new Domain.Entities.Fridge { Id = fridgeId, Name = "F", ModelId = modelId },
            new Product { Id = productId, Name = "P", DefaultQuantity = 0 },
            new FridgeProduct { Id = fridgeProductId, FridgeId = fridgeId, ProductId = productId, Quantity = 0 }
        );

        var service = new RestockService(ctx.Db, new FridgeProductService(ctx.Db));

        // When
        Func<Task> act = () => service.RestockZeroQuantityAsync(CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<BusinessRuleViolationException>()
            .WithMessage($"*Product '{productId}' has invalid default quantity*");

        (await ctx.Db.FridgeProducts.SingleAsync(x => x.Id == fridgeProductId))
            .Quantity.Should().Be(0);
    }


    [Fact]
    public async Task RestockZeroQuantityAsync_Should_Restock_And_Return_Processed_Count()
    {
        // Given
        await using var ctx = await SqlServerTestContext.CreateAsync(fx.ConnectionString);
        await CreateStoredProcedureAsync(ctx.Db.Database.GetConnectionString()!);

        var modelId = Guid.NewGuid();
        var fridgeId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var fridgeProductId = Guid.NewGuid();

        await ctx.Db.AddRangeAndSaveAsync(CancellationToken.None,
            new FridgeModel { Id = modelId, Name = "M" },
            new Domain.Entities.Fridge { Id = fridgeId, Name = "F", ModelId = modelId },
            new Product { Id = productId, Name = "P", DefaultQuantity = 5 },
            new FridgeProduct { Id = fridgeProductId, FridgeId = fridgeId, ProductId = productId, Quantity = 0 }
        );

        var service = new RestockService(ctx.Db, new FridgeProductService(ctx.Db));

        // When
        var processed = await service.RestockZeroQuantityAsync(CancellationToken.None);

        // Then
        processed.Should().Be(1);

        var fp = await ctx.Db.FridgeProducts.SingleAsync(x => x.Id == fridgeProductId);
        fp.Quantity.Should().Be(5);
    }

    private static async Task CreateStoredProcedureAsync(string connectionString)
    {
        var sql = @"
CREATE OR ALTER PROCEDURE dbo.sp_RestockZeroQuantity
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        fp.id AS fridge_product_id,
        fp.fridge_id AS fridge_id,
        fp.product_id AS product_id,
        ISNULL(p.default_quantity, 0) AS default_quantity
    FROM fridge_products fp
    JOIN products p ON p.id = fp.product_id
    WHERE fp.quantity = 0;
END";

        await using var conn = new SqlConnection(connectionString);
        await conn.OpenAsync();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        await cmd.ExecuteNonQueryAsync();
    }
}