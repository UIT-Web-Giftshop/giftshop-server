using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Application.Features.Objects.Queries.GetPagingObjects;
using Application.Features.Products.Vms;
using AutoMapper;
using Domain.Entities;
using Domain.Paging;
using Infrastructure.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Products.Queries.GetPagingProducts
{
    public class GetPagingProductsHandler : GetPagingObjectsHandler<Product, ProductVm>,
        IRequestHandler<GetPagingProductsQuery, ResponseApi<PagingModel<ProductVm>>>
    {
        public GetPagingProductsHandler(IProductRepository _productRepository, IMapper _mapper,
            ISaveFlagRepository _saveFlagRepository) : base(_productRepository, _mapper, 
            _saveFlagRepository)
        {

        }

        public override Expression<Func<Product, bool>> GetExpression(GetPagingObjectsQuery request)
        {
            return string.IsNullOrWhiteSpace(request.Search) ? null : p => 
                p.Name.Contains(request.Search);
        }

        public override Expression<Func<Product, object>> GetSortExpression(GetPagingObjectsQuery 
            request)
        {
            return q => q.Price;
        }

        public async Task<ResponseApi<PagingModel<ProductVm>>> Handle(GetPagingProductsQuery request,
           CancellationToken cancellationToken)
        {
            return await base.Handle(request, cancellationToken);
        }
    }
}