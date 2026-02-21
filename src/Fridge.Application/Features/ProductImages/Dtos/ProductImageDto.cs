namespace Fridge.Application.Features.ProductImages.Dtos;

public sealed record ProductImageDto(
    Guid Id,
    string FileName,
    string ContentType,
    long Size,
    bool IsPrimary,
    DateTimeOffset CreatedAt,
    string Url
);