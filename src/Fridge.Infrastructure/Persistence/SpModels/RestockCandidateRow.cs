namespace Fridge.Infrastructure.Persistence.SpModels;

public sealed class RestockCandidateRow
{
    public Guid FridgeProductId;
    public Guid ProductId;
    public Guid FridgeId;
    public int DefaultQuantity;
}