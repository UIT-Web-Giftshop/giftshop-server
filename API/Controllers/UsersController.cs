using System.Threading.Tasks;
using Application.Features.Users.Queries.GetOneUserByEmail;
using Application.Features.Users.Queries.GetOneUserById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
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
    }
}