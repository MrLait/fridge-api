using MediatR;

namespace Fridge.Application.Features.FridgeProducts.Commands.DeleteFridgeProduct;

public sealed record DeleteFridgeProductCommand(Guid Id) : IRequest;