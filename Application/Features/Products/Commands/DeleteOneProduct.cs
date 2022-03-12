using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Domain.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Products.Commands
{
    public class DeleteOneProductCommand : IRequest<ResponseApi<Unit>>
    {
        public string Id { get; init; }
    }
    
    public class DeleteOneProductCommandHandler : IRequestHandler<DeleteOneProductCommand, ResponseApi<Unit>>
    {
        private readonly IProductRepository _productRepository;

        public DeleteOneProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ResponseApi<Unit>> Handle(DeleteOneProductCommand request, CancellationToken cancellationToken)
        {
            Expression<Func<Product, bool>> expression = p => p.Id == request.Id;
            var existedProduct = await _productRepository.GetOneAsync(expression, cancellationToken);
            if (existedProduct == null)
            {
                return ResponseApi<Unit>.ResponseFail(ResponseConstants.ERROR_NOT_FOUND_ITEM);
            }
            var result = await _productRepository.DeleteOneAsync(expression, cancellationToken);
            return result 
                ? ResponseApi<Unit>.ResponseOk(Unit.Value, "Delete product success") 
                : ResponseApi<Unit>.ResponseFail(StatusCodes.Status500InternalServerError, ResponseConstants.ERROR_EXECUTING);
        }
    }
}