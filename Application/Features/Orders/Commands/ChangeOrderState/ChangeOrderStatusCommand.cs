using Application.Commons;
using MediatR;

namespace Application.Features.Orders.Commands.ChangeOrderState
{
    public class ChangeOrderStatusCommand : IRequest<ResponseApi<Unit>>
    {
        public string Id { get; set; }
        public string Status { get; set; }
    }
}