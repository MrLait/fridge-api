using Fridge.Api.Contracts.Fridges;
using Fridge.Application.Features.Fridges.Commands.AddProductToFridge;
using Fridge.Application.Features.Fridges.Commands.CreateFridge;
using Fridge.Application.Features.Fridges.Commands.DeleteFridge;
using Fridge.Application.Features.Fridges.Commands.UpdateFridge;
using Fridge.Application.Features.Fridges.Queries.GetFridgeById;
using Fridge.Application.Features.Fridges.Queries.GetFridgeProducts;
using Fridge.Application.Features.Fridges.Queries.GetFridges;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fridge.Api.Controllers;

[ApiController]
[Route("api/fridges")]
public class FridgesController(IMediator mediator) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetFridges(CancellationToken cancellationToken)
        => Ok(await mediator.Send(new GetFridgesQuery(), cancellationToken));

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetFridgeById([FromRoute] Guid id, CancellationToken ct)
            => Ok(await mediator.Send(new GetFridgeByIdQuery(id), ct));

    [AllowAnonymous]
    [HttpGet("{fridgeId:guid}/products")]
    public async Task<IActionResult> GetFridgeProducts([FromRoute] Guid fridgeId, CancellationToken cancellationToken)
        => Ok(await mediator.Send(new GetFridgeProductsQuery(fridgeId), cancellationToken));

    [HttpPost("{fridgeId:guid}/products")]
    public async Task<IActionResult> AddProductToFridge([FromRoute] Guid fridgeId, [FromBody] AddProductToFridgeRequest body, CancellationToken cancellationToken)
    {
        var id = await mediator.Send(new AddProductToFridgeCommand(fridgeId, body.ProductId, body.Quantity), cancellationToken);
        return Ok(new { fridgeProductId = id });
    }

    [HttpPost]
    public async Task<IActionResult> CreateFridge([FromBody] CreateFridgeRequest body, CancellationToken ct)
    {
        var initialProducts = body.InitialProducts?
            .Select(x => new InitialFridgeProductItem(x.ProductId, x.Quantity))
            .ToList();

        var id = await mediator.Send(new CreateFridgeCommand(body.Name, body.OwnerName, body.ModelId, initialProducts), ct);
        return CreatedAtAction(nameof(GetFridgeById), new { id }, new { id });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateFridge([FromRoute] Guid id, UpdateFridgeRequest body, CancellationToken ct)
    {
        await mediator.Send(new UpdateFridgeCommand(id, body.Name, body.OwnerName, body.ModelId), ct);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteFridge([FromRoute] Guid id, CancellationToken ct)
    {
        await mediator.Send(new DeleteFridgeCommand(id), ct);
        return NoContent();
    }
}
