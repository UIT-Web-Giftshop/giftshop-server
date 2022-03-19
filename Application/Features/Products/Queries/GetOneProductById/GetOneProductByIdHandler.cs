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

namespace Application.Features.Products.Queries.GetOneProductById
{
    public class GetOneProductByIdHandler : GetOneObjectHandler<Product>, 
        IRequestHandler<GetOneProductByIdQuery, ResponseApi<ProductVm>>
    {
        public GetOneProductByIdHandler(IProductRepository _productRepository, IMapper _mapper) : 
            base(_productRepository, _mapper)
        {

        }

        public async Task<ResponseApi<ProductVm>> Handle(GetOneProductByIdQuery request, 
            CancellationToken cancellationToken)
        {
            Expression<Func<Product, bool>> expression = p => p.Id == request.Id;
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