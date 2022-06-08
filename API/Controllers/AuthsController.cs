using System.Threading.Tasks;
using Application.Features.Auths.SigninUser;
using Application.Features.Auths.SignupUser;
using Application.Features.Auths.VerifyToken.ConfirmEmail;
using Application.Features.Profile.Commands.ForgetPassword;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [AllowAnonymous]
    public class AuthsController : BaseApiController
    {
        public AuthsController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("me/confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string token)
        {
            var data     = await _mediator.Send(new ConfirmEmailQuery { Token = token });
            return HandleResponseStatus(data);
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUpAccount(
            [FromBody] SignUpUserCommand command)
        {
            var data = await _mediator.Send(command);
            return HandleResponseStatus(data);
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(
            [FromBody] SignInUserCommand command)
        {
            var data     = await _mediator.Send(command);
            return HandleResponseStatus(data    );
        }
        
        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword(
            [FromBody] ForgetPasswordCommand command)
        {
            var data = await _mediator.Send(command);
            return HandleResponseStatus(data);
        }
    }
}