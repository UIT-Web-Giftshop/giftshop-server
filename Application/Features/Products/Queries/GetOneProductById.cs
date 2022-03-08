using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Application.Features.Products.Vms;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Products.Queries
{
    public class GetOneProductById
    {
        public class Query : IRequest<ResponseApi<ProductVm>>
        {
            public string Id { get; init; }
        }
        
        public class Handler : IRequestHandler<Query, ResponseApi<ProductVm>>
        {
            private readonly IProductRepository _productRepository;
            private readonly IMapper _mapper;

            public Handler(IProductRepository productRepository, IMapper mapper)
            {
                _productRepository = productRepository;
                _mapper = mapper;
            }

            public async Task<ResponseApi<ProductVm>> Handle(Query request, CancellationToken cancellationToken)
            {
                // repository action
                Expression<Func<Product, bool>> expression = p => p.Id == request.Id;
                var product = await _productRepository.GetOneAsync(expression, cancellationToken);
                
                // mapping
                var data = _mapper.Map<ProductVm>(product);
                
                return data != null 
                    ? ResponseApi<ProductVm>.ResponseOk(data) 
                    : ResponseApi<ProductVm>.ResponseFail(StatusCodes.Status404NotFound, ResponseConstants.ERROR_NOT_FOUND_ITEM);
            }
        }
    }
}