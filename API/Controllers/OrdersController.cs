#nullable enable
using System.Threading;
using System.Threading.Tasks;
using Application.Features.Orders.Queries.GetOneOrderById;
using Application.Features.Orders.Queries.GetPagingOrders;
using Application.Features.Orders.Vms;
using Domain.Entities.Order;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class OrdersController : ObjectsController<OrderVm, Order>
    {
        public OrdersController(IMediator _mediator) : base(_mediator)
        {

        }

        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetOneOrderById(string id)
        {
            var result = await _mediator.Send(new GetOneOrderByIdQuery() { Id = id });
            return HandleResponseStatus(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetPagingOrders([FromQuery] GetPagingOrdersQuery query,
            CancellationToken cancellationToken = default)
        {
            var result = await this._mediator.Send(query, cancellationToken);
            return HandleResponseStatus(result);
        }
    }
}