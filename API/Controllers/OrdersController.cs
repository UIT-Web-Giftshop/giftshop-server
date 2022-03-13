using System.Threading.Tasks;
using Application.Features.Orders.Queries.GetOneOrderById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class OrdersController : BaseApiController
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
    }
}