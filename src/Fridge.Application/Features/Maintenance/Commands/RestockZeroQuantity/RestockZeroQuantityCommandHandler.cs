using Fridge.Application.Common.Interfaces;
using MediatR;

namespace Fridge.Application.Features.Maintenance.Commands.RestockZeroQuantity;

public sealed class RestockZeroQuantityCommandHandler(IRestockService restockService)
    : IRequestHandler<RestockZeroQuantityCommand, int>
{
    public async Task<int> Handle(RestockZeroQuantityCommand request, CancellationToken cancellationToken)
        => await restockService.RestockZeroQuantityAsync(cancellationToken);
}