using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Features.Objects.Commands.Add;
using Application.Features.Objects.Commands.Delete.DeleteListObjects;
using Application.Features.Objects.Commands.Delete.DeleteOneObject;
using Application.Features.Objects.Commands.Update.UpdateOneObject;
using Application.Features.Objects.Vms;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public abstract class ObjectsController<T, V> : BaseApiController where T : ObjectVm where V : 
        IdentifiableObject
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOneObject(string id)
        {
            var result = await this._mediator.Send(new DeleteOneObjectCommand<V>() { Id = id });
            return HandleResponseStatus(result);
        }

        [HttpDelete("list")]
        public async Task<IActionResult> DeleteListObjects([FromBody] List<string> ids)
        {
            var result = await this._mediator.Send(new DeleteListObjectsCommand<V>() { Ids = ids });
            return HandleResponseStatus(result);
        }
    }
}