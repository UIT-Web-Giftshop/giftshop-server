using Application.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
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