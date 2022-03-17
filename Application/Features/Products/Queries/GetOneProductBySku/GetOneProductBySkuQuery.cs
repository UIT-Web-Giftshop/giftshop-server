using Application.Commons;
using Application.Features.Objects.Queries.GetOneObject;
using Application.Features.Products.Vms;
using MediatR;

namespace Application.Features.Products.Queries.GetOneProductBySku
{
    public class GetOneProductBySkuQuery : GetOneObjectQuery, IRequest<ResponseApi<ProductVm>>
    {
        public string Sku { get; init; }
    }
}