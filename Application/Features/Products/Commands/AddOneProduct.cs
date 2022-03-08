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
    public class AddOneProduct
    {
        public class Command : IRequest<ResponseApi<string>>
        {
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
                // Mapping
                var entity = _mapper.Map<Product>(request.Product);
                entity.CreatedAt = DateTime.Now;
                
                // Repository action
                var result = await _productRepository.AddAsync(entity, cancellationToken);
                
                return result == null 
                    ? ResponseApi<string>.ResponseFail(ResponseConstants.ERROR_EXECUTING) 
                    : ResponseApi<string>.ResponseOk(result.Id, "Add product success");
            }
        }
    }
}