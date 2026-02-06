
using MediatR;

namespace Fridge.Application.Features.Fridges.Commands.DeleteFridge;

public sealed record DeleteFridgeCommand(Guid Id, CancellationToken Ct)
    : IRequest;