using Fridge.Application.Features.Maintenance.Commands.RestockZeroQuantity;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fridge.Api.Controllers;

[ApiController]
[Route("api/maintenance")]
public sealed class MaintenanceController(IMediator mediator) : ControllerBase
{
    [HttpPost("restock-zero")]
    public async Task<IActionResult> RestockZero(CancellationToken ct)
    {
        var update = await mediator.Send(new RestockZeroQuantityCommand(), ct);
        return Ok(new { update });
    }
}