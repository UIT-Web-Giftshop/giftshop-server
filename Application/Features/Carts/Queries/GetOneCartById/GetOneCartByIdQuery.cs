using Application.Commons;
using Domain.ViewModels.Cart;
using MediatR;

namespace Application.Features.Carts.Queries.GetOneCartById
{
    public class GetOneCartByIdQuery : IRequest<ResponseApi<CartViewModel>>
    {
        public string Id { get; set; } // TODO: remove this later
    }
}