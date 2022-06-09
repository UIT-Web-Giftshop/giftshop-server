namespace Application.Features.Orders.Commands.CreateOrder
{
    public class MinimalProductForOrder
    {
        public string Sku { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
    }
}