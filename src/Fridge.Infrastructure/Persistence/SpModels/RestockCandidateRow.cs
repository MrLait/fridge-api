namespace Fridge.Infrastructure.Persistence.SpModels;

public sealed class RestockCandidateRow
{
    public Guid FridgeProductId { get; set; }
    public Guid ProductId { get; set; }
    public Guid FridgeId { get; set; }
    public int DefaultQuantity { get; set; }
}