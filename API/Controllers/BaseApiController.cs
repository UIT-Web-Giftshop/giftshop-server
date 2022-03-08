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

        protected BaseApiController(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected ActionResult HandleResponseStatus<T>(ResponseApi<T> responseApi)
        {
            if (responseApi.IsSuccess)
            {
                return Ok(responseApi);
            }
            
            //TODO: more handling
            return responseApi.Code switch
            {
                StatusCodes.Status404NotFound => NotFound(responseApi),
                StatusCodes.Status400BadRequest => BadRequest(responseApi),
                _ => BadRequest(responseApi)
            };
        }
    }
}