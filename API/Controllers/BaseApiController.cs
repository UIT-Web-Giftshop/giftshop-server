using Application.Commons;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        private IMediator mediator;
        protected IMediator _mediator => mediator ??= HttpContext
            .RequestServices
            .GetService<IMediator>();

        protected ActionResult HandleResponseStatus<T>(ResponseApi<T> responseApi)
        {
            if (responseApi.IsSuccess && responseApi.Code == 200)
            {
                return Ok(responseApi);
            }
            
            //TODO: more handling
            
            return BadRequest(responseApi);
        }
    }
}