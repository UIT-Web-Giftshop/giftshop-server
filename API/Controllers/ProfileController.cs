using System.Threading.Tasks;
using Application.Features.Profile.Commands.ChangePassword;
using Application.Features.Profile.Commands.UpdateProfileInfo;
using Application.Features.Profile.Queries.GetMyProfile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class ProfileController : BaseApiController
    {
        public ProfileController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            var data = await _mediator.Send(new GetMyProfileQuery());
            return HandleResponseStatus(data);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] ChangePasswordCommand command)
        {
            var data = await _mediator.Send(command);
            return HandleResponseStatus(data);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfileInfo(
            [FromBody] UpdateProfileInfoCommand command)
        {
            var data = await _mediator.Send(command);
            return HandleResponseStatus(data);
        }
    }
}