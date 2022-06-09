using System.Collections.Generic;
using Application.Commons;
using MediatR;

namespace Application.Features.Coupons.Commands.Delete
{
    public class DeleteCouponsCommand : IRequest<ResponseApi<Unit>>
    {
        public List<string> Ids { get; set; }
    }
}