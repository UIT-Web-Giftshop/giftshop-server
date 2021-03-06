#nullable enable
using System.ComponentModel;
using System.Threading.Tasks;
using Application.Features.Orders.Commands.ChangeOrderState;
using Application.Features.Orders.Commands.CreateOrder;
using Application.Features.Orders.Queries.GetOneOrderById;
using Application.Features.Orders.Queries.GetPagingOrders;
using Application.Features.Orders.Queries.GetReportOrder;
using CloudinaryDotNet.Actions;
using Domain.Entities.Account;
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
            [FromQuery] [DefaultValue("createdAt")] string? sortBy,
            [FromQuery] [DefaultValue(true)] bool isDesc)
        {
            var query = new GetPagingProfileOrdersQuery()
                { PagingRequest = pagingRequest, IsDesc = isDesc, SortBy = sortBy };
            var data = await _mediator.Send(query);
            return HandleResponseStatus(data);
        }

        [HttpGet("managed")]
        public async Task<IActionResult> GetPagingManagedOrders(
            [FromQuery] PagingRequest pagingRequest,
            [FromQuery] string? status,
            [FromQuery] string? userEmail,
            [FromQuery] [DefaultValue("createdAt")]
            string? sortBy,
            [FromQuery] [DefaultValue(true)] bool isDesc)
        {
            var query = new GetPagingManagedOrdersQuery()
            {
                PagingRequest = pagingRequest, IsDesc = isDesc, SortBy = sortBy, Status = status, FilterUser = userEmail
            };
            var data = await _mediator.Send(query);
            return HandleResponseStatus(data);
        }

        [Authorize(Roles = nameof(UserRoles.ADMIN))]
        [HttpGet("report")]
        public async Task<IActionResult> GetReportOrder(
            [FromQuery] GetReportOrderBetweenTimeQuery query)
        {
            var data = await _mediator.Send(query);
            return HandleResponseStatus(data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateProfileOrderCommand? command)
        {
            var data = await _mediator.Send(command);
            return HandleResponseStatus(data);
        }
        
        [HttpPut("{id}/status/{status}")]
        [Authorize(Roles = nameof(UserRoles.ADMIN))]
        public async Task<IActionResult> UpdateOrderStatus(string id, string status)
        {
            var command = new ChangeOrderStatusCommand() { Id = id, Status = status };
            var data = await _mediator.Send(command);
            return HandleResponseStatus(data);
        }
        
        [HttpPut("{id}/status/cancel")]
        public async Task<IActionResult> CancelOrder(string id)
        {
            var command = new CancelOrderCommand() { Id = id };
            var data = await _mediator.Send(command);
            return HandleResponseStatus(data);
        }

        [HttpPut("{id}/status/retrieve")]
        public async Task<IActionResult> RetrieveOrder(string id)
        {
            var command = new RetrieveOrderCommand() { Id = id };
            var data = await _mediator.Send(command);
            return HandleResponseStatus(data);
        }
    }
}