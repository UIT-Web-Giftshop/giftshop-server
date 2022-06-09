using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Features.Coupons.Commands;
using Application.Features.Coupons.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CouponsController : BaseApiController
    {
        public CouponsController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        public async Task<IActionResult> GenerateCoupons([FromBody] CreateCouponCommand command)
        {
            var data = await _mediator.Send(command);
            return Ok(data);
        }

        [HttpGet("info/{id}")]
        public async Task<IActionResult> GetOneCoupon(string id)
        {
            var data = await _mediator.Send(new GetCouponQuery() { Id = id });
            return HandleResponseStatus(data);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCoupons([FromBody] List<string> ids)
        {
            var data = await _mediator.Send(new DeleteCouponsCommand() { Ids = ids });
            return HandleResponseStatus(data);
        }
    }
}