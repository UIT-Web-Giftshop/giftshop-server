using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Application.Features.Coupons.Commands.CreateNew;
using Application.Features.Coupons.Commands.Delete;
using Application.Features.Coupons.Commands.UpdateCouponsPercent;
using Application.Features.Coupons.Queries;
using CloudinaryDotNet.Actions;
using Domain.Paging;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CouponsController : BaseApiController
    {
        public CouponsController(IMediator mediator) : base(mediator)
        {
        }

        [Authorize(Roles = "ADMIN")]
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
        
        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public async Task<IActionResult> GetPagingCoupon(
            [FromQuery] PagingRequest pagingRequest,
            [FromQuery] string eventCode,
            [FromQuery] [DefaultValue("percent")] string sortBy,
            [FromQuery] [DefaultValue(true)] bool isDesc,
            [FromQuery] [DefaultValue(true)] bool isActive)
        {
            var query = new GetPagingCouponQuery()
            {
                PagingRequest = pagingRequest, EventCode = eventCode, SortBy = sortBy, IsDesc = isDesc,
                IsActive = isActive
            };
            var data = await _mediator.Send(query);
            return HandleResponseStatus(data);
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateCouponsPercent(
            [FromBody] UpdateCouponsPercentCommand command)
        {
            var data = await _mediator.Send(command);
            return HandleResponseStatus(data);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete]
        public async Task<IActionResult> DeleteCoupons([FromBody] List<string> ids)
        {
            var data = await _mediator.Send(new DeleteCouponsCommand() { Ids = ids });
            return HandleResponseStatus(data);
        }
    }
}