namespace Fridge.Domain.Entities
{
    public class Fridge
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? OwnerName { get; set; } = string.Empty;

        // FK
        public Guid ModelId { get; set; }
        // Many to one
        public FridgeModel Model { get; set; } = null!;
        // One to many
        public ICollection<FridgeProduct> FridgeProducts { get; set; } = [];
    }
}