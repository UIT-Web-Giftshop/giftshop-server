using Application.Commons;
using MediatR;

namespace Application.Features.Orders.Commands.ChangeOrderState
{
    public class RetrieveOrderCommand : IRequest<ResponseApi<Unit>>
    {
        public string Id { get; set; }
    }
}