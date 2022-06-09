using Application.Commons;
using Domain.Paging;
using Domain.ViewModels.Product;
using MediatR;

namespace Application.Features.Products.Queries.GetPagingProducts
{
    public class GetPagingProductByTraitQuery : IRequest<ResponseApi<PagingModel<ProductDetailViewModel>>>
    {
        public PagingRequest PagingRequest { get; set; }
        public string Trait { get; set; }
        public string SortBy { get; set; }
        public bool IsDesc { get; set; }
    }
}