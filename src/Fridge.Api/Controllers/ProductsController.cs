using Fridge.Application.Features.Products.Commands.UpdateProduct;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fridge.Api.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController(IMediator mediator) : ControllerBase
    {
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateProductRequest body)
        {
            var dto = await mediator.Send(new UpdateProductCommand(id, body.Name, body.DefaultQuantity));
            return Ok(dto);
        }
    }
}