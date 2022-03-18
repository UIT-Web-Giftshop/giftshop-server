using System.Threading.Tasks;
using Application.Features.Objects.Commands.AddOneObject;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public abstract class ObjectsController<T> : BaseApiController
    {
        public ObjectsController(IMediator _mediator) : base(_mediator)
        {

        }

        [HttpPost]
        public async Task<IActionResult> AddOneObject([FromBody] T addedObjectVm)
        {
            var result = await this._mediator.Send(new AddOneObjectCommand<T>() { Data = addedObjectVm });
            return HandleResponseStatus(result);
        }
    }
}