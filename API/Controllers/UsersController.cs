#nullable enable
using System.Threading;
using System.Threading.Tasks;
using Application.Features.Users.Commands.AddOneUser;
using Application.Features.Users.Commands.DeleteOneUser;
using Application.Features.Users.Commands.UpdateOnePasswordUser;
using Application.Features.Users.Queries.GetOneUserByEmail;
using Application.Features.Users.Queries.GetOneUserById;
using Application.Features.Users.Queries.GetPagingUsers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize(Roles = "ADMIN")]

    public class UsersController : BaseApiController
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
        
        [HttpPost("new")]
        public async Task<IActionResult> AddOneUser(
            [FromBody] AddOneUserCommand command, 
            CancellationToken cancellationToken = default)
        {
            var result = await this._mediator.Send(command, cancellationToken);
            return HandleResponseStatus(result);
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateOneUserPassword(
            UpdateOneUserPasswordCommand command)
        {
            var result = await _mediator.Send(command);
            return HandleResponseStatus(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteOneUser(DeleteOneUserCommand command)
        {
            var result = await _mediator.Send(command);
            return HandleResponseStatus(result);
        }
    }
}