namespace Domain.Entities.Order
{
    public class OrderItem
    {
        public string ProductId { get; set; }

        public double Price { get; set; }

        public int Quantity { get; set; }
    }
}