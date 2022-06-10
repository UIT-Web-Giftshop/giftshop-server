using System.Threading;
using System.Threading.Tasks;
using Application.Features.Carts.Commands.UpdateOneCartItem;
using Application.Features.Carts.Queries.GetOneCart;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class CartsController : BaseApiController
    {
        public CartsController(IMediator _mediator) : base(_mediator)
        {
        }

        [HttpGet("")]
        public async Task<IActionResult> GetOneCartById()
        {
            var data = await _mediator.Send(
                new GetOneCartQuery());
            return HandleResponseStatus(data);
        }

        [HttpPut("add")]
        public async Task<IActionResult> UpdateAddCartItem(
            [FromBody] UpdateAddCartItemCommand command,
            CancellationToken cancellationToken)
        {
            var data = await _mediator.Send(command, cancellationToken);
            return HandleResponseStatus(data);
        }
        
        [HttpPut("remove")]
        public async Task<IActionResult> UpdateDeleteCartItem(
            [FromBody] UpdateDeleteCartItemCommand command,
            CancellationToken cancellationToken)
        {
            var data = await _mediator.Send(command, cancellationToken);
            return HandleResponseStatus(data);
        }
    }
}