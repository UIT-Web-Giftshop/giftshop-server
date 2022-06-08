using Application.Commons;
using Domain.ViewModels.Product;
using MediatR;

namespace Application.Features.Products.Queries.GetOneProductBySku
{
    public class GetOneProductBySkuQuery : IRequest<ResponseApi<ProductDetailViewModel>>
    {
        public string Sku { get; init; }
    }
}