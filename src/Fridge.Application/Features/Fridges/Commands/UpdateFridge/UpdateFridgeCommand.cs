
using MediatR;

namespace Fridge.Application.Features.Fridges.Commands.UpdateFridge;

public sealed record UpdateFridgeCommand(Guid Id, string Name, string? OwnerName, Guid ModelId)
    : IRequest;
