using System;
using System.Collections.Generic;
using System.ComponentModel;
using Application.Commons;
using Domain.Entities.Order;
using MediatR;

namespace Application.Features.Orders.Queries.GetReportOrder
{
    public class GetReportOrderBetweenTimeQuery : IRequest<ResponseApi<List<Order>>>
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string? Status { get; set; }
        [DefaultValue(true)] public bool AllStatus { get; set; }
    }
}