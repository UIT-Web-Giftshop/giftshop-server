using System.Threading;
using System.Threading.Tasks;
using Application.Features.Images.Commands;
using Application.Features.Images.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [AllowAnonymous]
    public class ImagesController : BaseApiController
    {
        private readonly IMediator _mediator;

        public ImagesController(IMediator mediator)
        {
            _mediator = mediator;
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

        [HttpGet("product/{key}")]
        public async Task<IActionResult> GetOneProductImage(
            string key,
            CancellationToken cancellationToken)
        {
            var result = await _mediator
                .Send(new GetOneProductImageQuery(){Key = key}, cancellationToken);
            return File(result.Data, "image/jpeg");
        }
    }
}