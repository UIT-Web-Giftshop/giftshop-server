using System.Threading.Tasks;
using Application.Features.Objects.Commands.Add;
using Application.Features.Objects.Commands.Update.UpdateOneObject;
using Application.Features.Objects.Vms;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public abstract class ObjectsController<T> : BaseApiController where T : ObjectVm
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOneObjectInfo(string id, [FromBody] T updatedObjectVm)
        {
            var result = await this._mediator.Send(new UpdateOneObjectCommand<T>() { Id = id, 
                Data = updatedObjectVm });
            return HandleResponseStatus(result);
        }
    }
}