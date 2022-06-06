using System.Threading.Tasks;
using Application.Features.Wishlist.Queries.GetOneWishList;
using Application.Features.Wishlists.Commands.UpdateOneWishlist;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class WishlistsController : BaseApiController
    {
        public WishlistsController(IMediator _mediator) : base(_mediator)
        {
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOneWishlist(string id)
        {
            var data = await  _mediator.Send(new GetOneWishlistQuery(){Id = id});
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