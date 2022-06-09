using Application.Commons;
using Domain.ViewModels.Wishlist;
using MediatR;

namespace Application.Features.Wishlists.Queries.GetOneWishList
{
    public class GetOneWishlistQuery : IRequest<ResponseApi<WishlistViewModel>>
    {
    }
}