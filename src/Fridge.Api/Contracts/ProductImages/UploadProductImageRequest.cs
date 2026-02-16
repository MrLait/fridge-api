namespace Fridge.Api.Contracts.ProductImages;

public sealed record UploadProductImageRequest
(
    IFormFile File
);