namespace Domain.ViewModels.Cart
{
    public class ProductInCart
    {
        public string Sku { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
    }
}