using Fridge.Application.Features.FridgeModels.Queries.GetFridgeModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fridge.Api.Controllers;

[ApiController]
[Route("api/fridge-models")]
public sealed class FridgeModelsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetFridgeModels(CancellationToken ct)
        => Ok(await mediator.Send(new GetFridgeModelsQuery(), ct));
}