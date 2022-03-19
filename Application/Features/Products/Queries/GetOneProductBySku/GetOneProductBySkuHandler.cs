using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Application.Features.Objects.Queries.GetOneObject;
using Application.Features.Products.Vms;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Products.Queries.GetOneProductBySku
{
    public class GetOneProductBySkuHandler : GetOneObjectHandler<Product>, 
        IRequestHandler<GetOneProductBySkuQuery, ResponseApi<ProductVm>>
    {
        public GetOneProductBySkuHandler(IProductRepository _productRepository, IMapper _mapper) :
            base(_productRepository, _mapper)
        {

        }
        public async Task<ResponseApi<ProductVm>> Handle(GetOneProductBySkuQuery request, 
            CancellationToken cancellationToken)
        {
            Expression<Func<Product, bool>> expression = p => p.Sku.Equals(request.Sku);
            var product = await this._baseRepository.GetOneAsync(expression, cancellationToken);

            if (product == null)
            {
                return ResponseApi<ProductVm>.ResponseFail(StatusCodes.Status400BadRequest, 
                    ResponseConstants.ERROR_NOT_FOUND_ITEM);
            }

            var data = this._mapper.Map<ProductVm>(product);
            return ResponseApi<ProductVm>.ResponseOk(data);
        }
    }
}