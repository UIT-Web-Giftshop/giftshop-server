using System.Collections.Generic;

namespace Domain.ViewModels.Wishlist
{
    public class WishlistViewModel
    {
        public string Id { get; set; }
        public IList<ProductInWishlist> Products { get; set; }
    }
}