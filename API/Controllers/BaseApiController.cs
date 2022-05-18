using API.Commons;
using Application.Commons;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors(Constants.CORS_ANY_ORIGIN_POLICY)]
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
                return Ok(responseApi);
            }
            
            //TODO: more handling
            return responseApi.Status switch
            {
                StatusCodes.Status404NotFound => NotFound(responseApi),
                StatusCodes.Status400BadRequest => BadRequest(responseApi),
                _ => BadRequest(responseApi)
            };
        }
    }
}