using System.Threading.Tasks;
using Application.Features.Auths.SignupUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AuthsController : BaseApiController
    {
        public AuthsController(IMediator _mediator) : base(_mediator)
        {
        }

        [AllowAnonymous]
        [HttpPost("signup")]
        public async Task<IActionResult> SignUpAccount(
            [FromBody] SignupUserCommand command)
        {
            var response = await _mediator.Send(command);
            return HandleResponseStatus(response);
        }

        [AllowAnonymous]
        [HttpPost("signin")]
        public async Task<IActionResult> SignIn()
        {
            return Ok("signin");
        }
    }
}