using Application.Commons;
using Domain.ViewModels.Wishlist;
using MediatR;

namespace Application.Features.Wishlist.Queries.GetOneWishList
{
    public class GetOneWishlistQuery : IRequest<ResponseApi<WishlistViewModel>>
    {
        public string Id { get; set; } // TODO: remove this
    }
}