using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Application.Features.Products.Vms;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Products.Commands
{
    public class UpdateOneProductInfoCommand : IRequest<ResponseApi<Unit>>
    {
        public string Id { get; init; }
        public ProductVm Product { get; init; }
    }
    
    public class UpdateOneProductInfoCommandHandler : IRequestHandler<UpdateOneProductInfoCommand, ResponseApi<Unit>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public UpdateOneProductInfoCommandHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ResponseApi<Unit>> Handle(UpdateOneProductInfoCommand request, CancellationToken cancellationToken)
        {
            Expression<Func<Product, bool>> expression = p => p.Id == request.Id;
            var existedProduct = await _productRepository.GetOneAsync(expression, cancellationToken);
            if (existedProduct == null)
            {
                return ResponseApi<Unit>.ResponseFail(ResponseConstants.ERROR_NOT_FOUND_ITEM);
            }

            _mapper.Map<ProductVm, Product>(request.Product, existedProduct);
            existedProduct.UpdateAt = DateTime.UtcNow;
                
            var result = await _productRepository.UpdateAsync(expression, existedProduct, cancellationToken);
            return result 
                ? ResponseApi<Unit>.ResponseOk(Unit.Value, "Update product success") 
                : ResponseApi<Unit>.ResponseFail(ResponseConstants.ERROR_EXECUTING);

        }
    }
}