using System.Threading.Tasks;
using Application.Features.Auths.ForgetPassword;
using Application.Features.Auths.ResendConfirmEmail;
using Application.Features.Auths.SigninUser;
using Application.Features.Auths.SignupUser;
using Application.Features.Auths.VerifyToken.ConfirmEmail;
using Application.Features.Auths.VerifyToken.ConfirmResetPassword;
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
        
        [HttpGet("me/resend-confirm-email")]
        public async Task<IActionResult> ResendConfirmEmail([FromQuery] string email)
        {
            var data = await _mediator.Send(new ResendConfirmEmailCommand { Email = email });
            return HandleResponseStatus(data);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> RestPassword([FromBody] ConfirmResetPasswordCommand command)
        {
            var data = await _mediator.Send(command);
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
            return HandleResponseStatus(data);
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