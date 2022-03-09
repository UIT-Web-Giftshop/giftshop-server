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
    public class GetOneProductByIdQuery : IRequest<ResponseApi<ProductVm>>
    {
        public string Id { get; init; }
    }
    
    public class GetOneProductByIdHandler : IRequestHandler<GetOneProductByIdQuery, ResponseApi<ProductVm>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        
        public GetOneProductByIdHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ResponseApi<ProductVm>> Handle(GetOneProductByIdQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Product, bool>> expression = p => p.Id == request.Id;
            var product = await _productRepository.GetOneAsync(expression, cancellationToken);
                
            // mapping & return
            if (product == null)
                return ResponseApi<ProductVm>.ResponseFail(StatusCodes.Status400BadRequest,
                    ResponseConstants.ERROR_NOT_FOUND_ITEM);
                
            var data = _mapper.Map<ProductVm>(product);
            return ResponseApi<ProductVm>.ResponseOk(data);
        }
    }
}