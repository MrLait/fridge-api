using Fridge.Api.Contracts.ProductImages;
using Fridge.Application.Features.ProductImages.Commands.DeleteProductImage;
using Fridge.Application.Features.ProductImages.Commands.SetPrimaryProductImage;
using Fridge.Application.Features.ProductImages.Commands.UploadProductImage;
using Fridge.Application.Features.ProductImages.Queries.GetProductImages;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fridge.Api.Controllers;

[ApiController]
[Route("api/products/{productId:guid}/images")]
public sealed class ProductImagesController(IMediator mediator) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetProductImages([FromRoute] Guid productId, CancellationToken ct)
    {
        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var items = await mediator.Send(new GetProductImagesQuery(productId, baseUrl), ct);
        return Ok(items);
    }

    [AllowAnonymous]
    [HttpGet("{imageId:guid}")]
    public async Task<IActionResult> GetFile(
        [FromRoute] Guid productId,
        [FromRoute] Guid imageId,
        [FromServices] Application.Common.Interfaces.IAppDbContext db,
        [FromServices] Application.Common.Interfaces.IFileStorage storage,
        CancellationToken ct)
    {
        var img = await EntityFrameworkQueryableExtensions
            .SingleOrDefaultAsync(db.ProductImages.Where(x => x.ProductId == productId && x.Id == imageId), ct);

        if (img is null) return NotFound();

        if (!await storage.ExistsAsync(img.StorageKey, ct))
            return NotFound();

        var stream = await storage.OpenReadAsync(img.StorageKey, ct);
        return File(stream, img.ContentType, enableRangeProcessing: true);
    }

    [HttpPost]
    [RequestSizeLimit(5 * 1024 * 1024)]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Upload(
        [FromRoute] Guid productId,
        [FromForm] UploadProductImageRequest request,
        CancellationToken ct)
    {
        if (request.File is null || request.File.Length == 0)
            return BadRequest("File is empty.");

        await using var s = request.File.OpenReadStream();

        var imageId = await mediator.Send(new UploadProductImageCommand(
            productId, request.File.FileName, request.File.ContentType, request.File.Length, s), ct);

        return Ok(new { imageId });
    }

    [HttpPut("{imageId:guid}/primary")]
    public async Task<IActionResult> SetPrimary([FromRoute] Guid productId, [FromRoute] Guid imageId, CancellationToken ct)
    {
        await mediator.Send(new SetPrimaryProductImageCommand(productId, imageId), ct);
        return NoContent();
    }

    [HttpDelete("{imageId:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid productId, [FromRoute] Guid imageId, CancellationToken ct)
    {
        await mediator.Send(new DeleteProductImageCommand(productId, imageId), ct);
        return NoContent();
    }
}