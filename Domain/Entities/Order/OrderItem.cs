namespace Domain.Entities.Order
{
    public class OrderItem
    {
        public string ProductSku { get; set; }

        public double Price { get; set; }

        public int Quantity { get; set; }

        public double Sum()
        {
            return Price * Quantity;
        }
    }
}