using Fridge.Application.Features.Products.Commands.UpdateProduct;
using Fridge.Application.Features.Products.Queries.GetProductById;
using Fridge.Application.Features.Products.Queries.GetProducts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fridge.Api.Controllers
{
    [ApiController]
    [Route("api/products")]
    public sealed class ProductsController(IMediator mediator) : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var products = await mediator.Send(new GetProductsQuery(), ct);
            return Ok(products);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetProduct([FromRoute] Guid id, CancellationToken ct)
            => Ok(await mediator.Send(new GetProductByIdQuery(id), ct));

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateProductRequest body)
        {
            var dto = await mediator.Send(new UpdateProductCommand(id, body.Name, body.DefaultQuantity));
            return Ok(dto);
        }
    }
}