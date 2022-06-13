namespace Domain.Entities.Order
{
    public class OrderItem
    {
        public string Sku { get; set; }

        public double Price { get; set; }

        public int Quantity { get; set; }
        
        public string Name { get; set; }
        
        public string ImageUrl { get; set; }

        public double Sum()
        {
            return Price * Quantity;
        }
    }
}