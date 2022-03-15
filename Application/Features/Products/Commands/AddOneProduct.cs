using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Application.Features.Products.Vms;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Products.Commands
{
    public class AddOneProductCommand : IRequest<ResponseApi<string>>
    {
        public ProductVm Product { get; init; }
    }
    
    public class AddOneProductCommandHandler : IRequestHandler<AddOneProductCommand, ResponseApi<string>>
    {
        private readonly IProductRepository _productRepository;
        private readonly ISaveFlagRepository _saveFlagRepository;
        private readonly IMapper _mapper;

        public AddOneProductCommandHandler(IProductRepository productRepository, IMapper mapper, ISaveFlagRepository saveFlagRepository)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _saveFlagRepository = saveFlagRepository;
        }

        public async Task<ResponseApi<string>> Handle(AddOneProductCommand request, CancellationToken cancellationToken)
        {
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