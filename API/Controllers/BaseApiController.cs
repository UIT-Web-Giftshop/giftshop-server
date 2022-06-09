using Application.Commons;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        protected readonly IMediator _mediator;

        public BaseApiController(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected ActionResult HandleResponseStatus<T>(ResponseApi<T> responseApi, string redirect = null)
        {
            if (responseApi.Success)
            {
                return responseApi.Status switch
                {
                    StatusCodes.Status302Found => Redirect(redirect),
                    _ => StatusCode(responseApi.Status, responseApi)
                };
            }
            
            //TODO: more handling, refactor status code response
            return responseApi.Status switch
            {
                StatusCodes.Status404NotFound => NotFound(responseApi),
                StatusCodes.Status400BadRequest => BadRequest(responseApi),
                StatusCodes.Status403Forbidden => Forbid(),
                StatusCodes.Status503ServiceUnavailable => StatusCode(StatusCodes.Status503ServiceUnavailable),
                _ => StatusCode(StatusCodes.Status500InternalServerError, responseApi)
            };
        }
    }
}