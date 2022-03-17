using Application.Commons;
using Application.Features.Objects.Queries.GetOneObject;
using Application.Features.Products.Vms;
using MediatR;

namespace Application.Features.Products.Queries.GetOneProductById
{
    public class GetOneProductByIdQuery : GetOneObjectByIdQuery, IRequest<ResponseApi<ProductVm>>
    {

    }
}