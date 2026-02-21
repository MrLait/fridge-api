using Fridge.Application.Common.Interfaces;
using MediatR;

namespace Fridge.Application.Features.Fridges.Commands.AddProductToFridge;

public sealed class AddProductToFridgeCommandHandler(IFridgeProductService service)
    : IRequestHandler<AddProductToFridgeCommand, Guid>
{
    public async Task<Guid> Handle(AddProductToFridgeCommand request, CancellationToken ct)
        => await service.AddProductAsync(request.FridgeId, request.ProductId, request.Quantity, true, ct);
}