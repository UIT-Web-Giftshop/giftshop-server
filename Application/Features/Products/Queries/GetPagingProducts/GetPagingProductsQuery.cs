using Application.Commons;
using Application.Features.Objects.Queries.GetPagingObjects;
using Application.Features.Products.Vms;
using Domain.Paging;
using MediatR;

namespace Application.Features.Products.Queries.GetPagingProducts
{
    public class GetPagingProductsQuery : GetPagingObjectsQuery, 
        IRequest<ResponseApi<PagingModel<ProductVm>>>
    {

    }
}