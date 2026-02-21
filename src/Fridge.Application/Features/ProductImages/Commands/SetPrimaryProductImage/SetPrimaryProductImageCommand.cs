using MediatR;

namespace Fridge.Application.Features.ProductImages.Commands.SetPrimaryProductImage;

public sealed record SetPrimaryProductImageCommand(Guid ProductId, Guid ImageId) : IRequest;