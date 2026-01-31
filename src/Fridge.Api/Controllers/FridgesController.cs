using Fridge.Application.Features.Fridges.Commands.AddProductToFridge;
using Fridge.Application.Features.Fridges.Queries.GetFridgeProducts;
using Fridge.Application.Features.Fridges.Queries.GetFridges;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fridge.Api.Controllers;

[ApiController]
[Route("api/fridges")]
public class FridgesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        => Ok(await mediator.Send(new GetFridgesQuery(), cancellationToken));

    [HttpGet("{fridgeId:guid}/products")]
    public async Task<IActionResult> GetProducts([FromRoute] Guid fridgeId, CancellationToken cancellationToken)
        => Ok(await mediator.Send(new GetFridgeProductsQuery(fridgeId), cancellationToken));

    [HttpPost("{fridgeId:guid}/products")]
    public async Task<IActionResult> AddProduct([FromRoute] Guid fridgeId, [FromBody] AddProductToFridgeRequest body, CancellationToken cancellationToken)
    {
        var id = await mediator.Send(new AddProductToFridgeCommand(fridgeId, body.ProductId, body.Quantity), cancellationToken);
        return Ok(new { fridgeProductId = id });
    }
}