using MediatR;

namespace Fridge.Application.Features.Maintenance.Commands.RestockZeroQuantity;

public sealed record RestockZeroQuantityCommand : IRequest<int>;