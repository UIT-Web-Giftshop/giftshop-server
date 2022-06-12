using System.ComponentModel;
using Application.Commons;
using Domain.Entities;
using Domain.Paging;
using MediatR;

namespace Application.Features.Products.Queries.GetPagingProducts
{
    public class GetPagingProductsManagedQuery : IRequest<ResponseApi<PagingModel<Product>>>
    {
        [DefaultValue(1)] public int PageIndex { get; set; } = 1;

        [DefaultValue(20)] public int PageSize { get; set; } = 20;
        
        public string Search { get; set; }
        
        [DefaultValue("price")] public string SortBy { get; set; }
        
        [DefaultValue(true)] public bool IsDesc { get; set; }
        
        [DefaultValue("all")] public string ActiveStatus { get; set; }
    }
}