#nullable enable
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.Users.Commands.Update.UpdateOneFiledOfUser.UpdatePasswordOfUser;
using Application.Features.Users.Queries.GetOneUserByEmail;
using Application.Features.Users.Queries.GetOneUserById;
using Application.Features.Users.Queries.GetPagingUsers;
using Application.Features.Users.Vms;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class UsersController : ObjectsController<UserVm, User>
    {
        public UsersController(IMediator _mediator) : base(_mediator)
        {

        }

        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetOneUserById(string id)
        {
            var result = await this._mediator.Send(new GetOneUserByIdQuery() { Id = id });
            return HandleResponseStatus(result);
        }

        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetOneUserByEmail(string email)
        {
            var result = await this._mediator.Send(new GetOneUserByEmailQuery() { Email = email });
            return HandleResponseStatus(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetPagingUsers([FromQuery] GetPagingUsersQuery query,
            CancellationToken cancellationToken = default)
        {
            var result = await this._mediator.Send(query, cancellationToken);
            return HandleResponseStatus(result);
        }
        
        [HttpPatch("{id}/password/{password}")]
        public async Task<IActionResult> UpdatePasswordOfUser(string id, string password)
        {
            var result = await this._mediator.Send(new UpdatePasswordOfUserCommand() { Id = id, Password 
                = password });
            return HandleResponseStatus(result);
        }

        [HttpPost]
        public override async Task<IActionResult> AddOneObject([FromBody] UserVm addedUserVm)
        {
            return await base.AddOneObject(addedUserVm);
        }

        [HttpPut("{id}")]
        public override async Task<IActionResult> UpdateOneObjectInfo(string id,
            [FromBody] UserVm updatedUserVm)
        {
            return await base.UpdateOneObjectInfo(id, updatedUserVm);
        }

        [HttpDelete("{id}")]
        public override async Task<IActionResult> DeleteOneObject(string id)
        {
            return await base.DeleteOneObject(id);
        }

        [HttpDelete("list")]
        public override async Task<IActionResult> DeleteListObjects([FromBody] List<string> ids)
        {
            return await base.DeleteListObjects(ids);
        }
    }
}