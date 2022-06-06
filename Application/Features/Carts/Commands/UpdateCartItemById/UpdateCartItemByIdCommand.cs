using Application.Commons;
using MediatR;

namespace Application.Features.Carts.Commands.UpdateCartItemById
{
    public class UpdateCartItemByIdCommand : IRequest<ResponseApi<Unit>>
    {
        public string Id { get; init; }
        public string ProductId { get; init; }
        public int Quantity { get; init; }
    }
}