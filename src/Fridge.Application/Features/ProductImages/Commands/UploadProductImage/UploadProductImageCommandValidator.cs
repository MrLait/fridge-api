using FluentValidation;

namespace Fridge.Application.Features.ProductImages.Commands.UploadProductImage;

public sealed class UploadProductImageCommandValidator
    : AbstractValidator<UploadProductImageCommand>
{
    public UploadProductImageCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.FileName).NotEmpty();
        RuleFor(x => x.ContentType).Must(ct => ct is "image/jpeg" or "image/png");
        RuleFor(x => x.Size).GreaterThan(0).LessThan(5 * 1024 * 1024);
    }
}