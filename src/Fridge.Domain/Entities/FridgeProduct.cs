namespace Fridge.Domain.Entities
{
    public class FridgeProduct
    {
        public Guid Id { get; set; }
        // FK
        public Guid ProductId { get; set; }
        // Many to one
        public Product Product { get; set; } = null!;
        // FK
        public Guid FridgeId { get; set; }
        // Many to one
        public Fridge Fridge { get; set; } = null!;

        public int Quantity { get; set; }
    }
}