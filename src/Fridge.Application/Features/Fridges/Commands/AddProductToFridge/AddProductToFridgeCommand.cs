using MediatR;

namespace Fridge.Application.Features.Fridges.Commands.AddProductToFridge;

public sealed record AddProductToFridgeCommand(Guid FridgeId, Guid ProductId, int Quantity) : IRequest<Guid>;