namespace Fridge.Domain.Entities;

public class FridgeModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? Year { get; set; }

    // One to many
    public ICollection<Fridge> Fridges { get; set; } = new List<Fridge>();
}