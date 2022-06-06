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

        [HttpPut]
        public async Task<IActionResult> UpdateCartItem(
            [FromBody] UpdateCartItemByIdCommand command,
            CancellationToken cancellationToken)
        {
            var data = await _mediator.Send(command, cancellationToken);
            return HandleResponseStatus(data);
        }
    }
}