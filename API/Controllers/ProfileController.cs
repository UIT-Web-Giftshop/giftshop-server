using System.Threading.Tasks;
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
    }
}