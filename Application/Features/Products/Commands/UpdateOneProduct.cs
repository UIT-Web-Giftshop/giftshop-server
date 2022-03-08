using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Application.Features.Products.Vms;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Products.Commands
{
    public class UpdateOneProduct
    {
        public class Command : IRequest<ResponseApi<string>>
        {
            public string Id { get; init; }
            public ProductVm Product { get; init; }
        }
        
        public class Handler : IRequestHandler<Command, ResponseApi<string>>
        {
            private readonly IProductRepository _productRepository;
            private readonly IMapper _mapper;

            public Handler(IProductRepository productRepository, IMapper mapper)
            {
                _productRepository = productRepository;
                _mapper = mapper;
            }

            public async Task<ResponseApi<string>> Handle(Command request, CancellationToken cancellationToken)
            {
                var existedProduct = await _productRepository.GetOneAsync(p => p.Id == request.Id, cancellationToken);
                if (existedProduct == null)
                {
                    return ResponseApi<string>.ResponseFail(ResponseConstants.ERROR_NOT_FOUND_ITEM);
                }

                _mapper.Map<ProductVm, Product>(request.Product, existedProduct);
                existedProduct.UpdateAt = DateTime.UtcNow;
                
                var result = await _productRepository.UpdateAsync(existedProduct, cancellationToken);
                return result 
                    ? ResponseApi<string>.ResponseOk(existedProduct.Id, "Update product success") 
                    : ResponseApi<string>.ResponseFail(ResponseConstants.ERROR_EXECUTING);
            }
        }
    }
}