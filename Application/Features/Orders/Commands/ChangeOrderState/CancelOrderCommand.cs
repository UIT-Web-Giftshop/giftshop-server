using Application.Commons;
using MediatR;

namespace Application.Features.Orders.Commands.ChangeOrderState
{
    public class CancelOrderCommand : IRequest<ResponseApi<Unit>>
    {
        public string Id { get; set; }
    }
}