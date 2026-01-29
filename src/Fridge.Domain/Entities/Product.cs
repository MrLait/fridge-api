namespace Fridge.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int? DefaultQuantity { get; set; }

        public ICollection<FridgeProduct> FridgeProducts { get; set; } = new List<FridgeProduct>();
    }
}