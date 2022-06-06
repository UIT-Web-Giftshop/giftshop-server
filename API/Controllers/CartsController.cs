using System.Threading;
using System.Threading.Tasks;
using Application.Features.Carts.Commands.UpdateCartItemById;
using Application.Features.Carts.Queries.GetOneCartById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CartsController : BaseApiController
    {
        public CartsController(IMediator _mediator) : base(_mediator)
        {
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOneCartById(string id)
        {
            var data = await _mediator.Send(
                new GetOneCartByIdQuery { Id = id });
            return HandleResponseStatus(data);
        }

        [HttpPut("/add")]
        public async Task<IActionResult> UpdateAddCartItem(
            [FromBody] UpdateAddCartItemCommand command,
            CancellationToken cancellationToken)
        {
            var data = await _mediator.Send(command, cancellationToken);
            return HandleResponseStatus(data);
        }

        [HttpPut("/remove")]
        public async Task<IActionResult> UpdateDeleteCartItem(
            [FromBody] UpdateDeleteCartItemCommand command,
            CancellationToken cancellationToken)
        {
            var data = await _mediator.Send(command, cancellationToken);
            return HandleResponseStatus(data);
        }
    }
}