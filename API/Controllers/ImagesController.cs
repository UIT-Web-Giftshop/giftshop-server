using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.Images.Commands;
using Application.Features.Images.Commands.ProductImage;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [AllowAnonymous]
    public class ImagesController : BaseApiController
    {
        public ImagesController(IMediator mediator) : base(mediator) { }

        [HttpPost("product/{sku}")]
        public async Task<IActionResult> AddOneProductImage(
            [FromRoute] string sku,
            [FromForm] IFormFile file,
            CancellationToken cancellationToken)
        {
            var request = new AddOneProductImageCommand { Sku = sku, File = file };
            var data = await _mediator.Send(request, cancellationToken);
            return HandleResponseStatus(data);
        }
    }
}