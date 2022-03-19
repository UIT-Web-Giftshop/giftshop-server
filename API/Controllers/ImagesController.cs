using System.Threading;
using System.Threading.Tasks;
using Application.Features.Images.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [AllowAnonymous]
    public class ImagesController : BaseApiController
    {
        //private readonly IMediator mediator;

        public ImagesController(IMediator _mediator) : base(_mediator)
        {
            //this.mediator = _mediator;
        }

        [HttpPost("product/{productId}")]
        public async Task<IActionResult> AddOneProductImage(
            string productId,
            [FromForm] IFormFile file,
            CancellationToken cancellationToken)
        {
            var request = new AddOneProductImageQuery() { ProductId = productId, File = file };
            var result = await _mediator.Send(request, cancellationToken);
            return HandleResponseStatus(result);
        }
    }
}