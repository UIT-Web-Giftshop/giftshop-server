using System.Threading.Tasks;
using Application.Features.Wishlists.Commands.UpdateOneWishlist;
using Application.Features.Wishlists.Queries.GetOneWishList;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class WishlistsController : BaseApiController
    {
        public WishlistsController(IMediator _mediator) : base(_mediator)
        {
        }

        [HttpGet("")]
        public async Task<IActionResult> GetOneWishlist()
        {
            var data = await  _mediator.Send(new GetOneWishlistQuery());
            return HandleResponseStatus(data);
        }

        [HttpPost]
        public async Task<IActionResult> AddOneToWishlist([FromBody] UpdateAddWishlistItemCommand command)
        {
            var data = await _mediator.Send(command);
            return HandleResponseStatus(data);
        }
        
        [HttpDelete]
        public async Task<IActionResult> DeleteOneFromWishlist([FromBody] UpdateDeleteWishlistItemCommand command)
        {
            var data = await _mediator.Send(command);
            return HandleResponseStatus(data);
        }
    }
}