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
        protected IMediator _mediator;

        protected BaseApiController(IMediator mediator)
        {
            _mediator = mediator;
        }

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