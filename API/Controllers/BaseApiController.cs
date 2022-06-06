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

        public BaseApiController(IMediator _mediator)
        {
            this._mediator = _mediator;
        }

        protected ActionResult HandleResponseStatus<T>(ResponseApi<T> responseApi)
        {
            if (responseApi.Success)
            {
                return StatusCode(responseApi.Status, responseApi.Data);
            }
            
            //TODO: more handling, refactor status code response
            return responseApi.Status switch
            {
                StatusCodes.Status404NotFound => NotFound(responseApi.Message),
                StatusCodes.Status400BadRequest => BadRequest(responseApi.Message),
                _ => BadRequest(responseApi.Message)
            };
        }
    }
}