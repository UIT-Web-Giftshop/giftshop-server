#nullable enable
using System.Threading;
using System.Threading.Tasks;
using Application.Features.Users.Queries.GetOneUserByEmail;
using Application.Features.Users.Queries.GetOneUserById;
using Application.Features.Users.Queries.GetPagingUsers;
using Application.Features.Users.Vms;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class UsersController : ObjectsController<UserVm>
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
    }
}