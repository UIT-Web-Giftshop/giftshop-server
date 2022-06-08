#nullable enable
using System.ComponentModel;
using System.Threading.Tasks;
using Application.Features.Orders.Commands.ChangeOrderState;
using Application.Features.Orders.Commands.CreateOrder;
using Application.Features.Orders.Queries.GetOneOrderById;
using Application.Features.Orders.Queries.GetPagingOrders;
using Domain.Paging;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class OrdersController : BaseApiController
    {
        public OrdersController(IMediator mediator) : base(mediator)
        {
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetOneProfileOrder(string id)
        {
            var data = await _mediator.Send(new GetOneProfileOrderQuery() { Id = id });
            return HandleResponseStatus(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetPagingProfileOrder(
            [FromQuery] PagingRequest pagingRequest,
            [FromQuery] [DefaultValue("createdAt")] string sortBy,
            [FromQuery] [DefaultValue(true)] bool isDesc)
        {
            var query = new GetPagingProfileOrdersQuery()
                { PagingRequest = pagingRequest, IsDesc = isDesc, SortBy = sortBy };
            var data = await _mediator.Send(query);
            return HandleResponseStatus(data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateProfileOrderCommand command)
        {
            var data = await _mediator.Send(command);
            return HandleResponseStatus(data);
        }
        
        [HttpPut("{id}/status/{status}")]
        public async Task<IActionResult> UpdateOrderStatus(string id, string status)
        {
            var command = new ChangeOrderStatusCommand() { Id = id, Status = status };
            var data = await _mediator.Send(command);
            return HandleResponseStatus(data);
        }
    }
}