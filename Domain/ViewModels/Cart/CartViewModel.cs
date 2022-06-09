using System.Collections.Generic;

namespace Domain.ViewModels.Cart
{
    public class CartViewModel
    {
        public string Id { get; set; } // Id cart
        public IList<ProductInCart> Products { get; set; } // list of products in cart
    }
}