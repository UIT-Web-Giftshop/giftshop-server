#nullable enable
using Application.Commons;
using Domain.Entities;
using Domain.Paging;
using MediatR;

namespace Application.Features.Coupons.Queries
{
    public class GetPagingCouponQuery : IRequest<ResponseApi<PagingModel<Coupon>>>
    {
        public PagingRequest PagingRequest { get; set; }
        public string? EventCode { get; set; }
        public string SortBy { get; set; }
        public bool IsDesc { get; set; }
        
        public bool IsActive { get; set; }
    }
}