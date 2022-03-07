using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Domain.Entities;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.Features.Products.Queries
{
    public class GetOneProductById
    {
        public class Query : IRequest<ResponseApi<Product>>
        {
            public string Id { get; init; }
        }
        
        public class Handler : IRequestHandler<Query, ResponseApi<Product>>
        {
            private readonly IProductRepository _productRepository;

            public Handler(IProductRepository productRepository)
            {
                _productRepository = productRepository;
            }

            public async Task<ResponseApi<Product>> Handle(Query request, CancellationToken cancellationToken)
            {
                Expression<Func<Product, bool>> expression = p => p.Id == request.Id;
                var data = await _productRepository.GetOneAsync(expression, cancellationToken);
                return data != null 
                    ? ResponseApi<Product>.ResponseOk(data) 
                    : ResponseApi<Product>.ResponseFail(ResponseMessages.ERROR_NOT_FOUND_ITEM);
            }
        }
    }
}