using Fridge.Application.Features.FridgeProducts.Commands.DeleteFridgeProduct;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fridge.Api.Controllers;

[ApiController]
[Route("api/fridge-products")]
public sealed class FridgeProductsController(IMediator mediator) : ControllerBase
{
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteFridgeProductCommand(id), cancellationToken);
        return NoContent();
    }
}