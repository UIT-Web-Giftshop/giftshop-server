using System.Collections.Generic;

namespace Domain.ViewModels.Cart
{
    public class CartViewModel
    {
        public string Id { get; set; }
        public IList<ProductInCart> Products { get; set; }
    }
}