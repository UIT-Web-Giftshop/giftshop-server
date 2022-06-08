using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Application.Features.Products.Commands.UpdateOneProductStock;
using Infrastructure.Extensions.Mongo;
using Infrastructure.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;

namespace Application.Features.Products.Commands
{
    public class UpdateProductStockCommandHandler : IRequestHandler<UpdateOneProductStockCommand, ResponseApi<Unit>>
    {
        private readonly IProductRepository _productRepository;
    
        public UpdateProductStockCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
    
        public async Task<ResponseApi<Unit>> Handle(UpdateOneProductStockCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.FindOneAsync(x => x.Sku == request.Sku, cancellationToken);
            if (product == null)
            {
                return ResponseApi<Unit>.ResponseFail(ResponseConstants.ERROR_NOT_FOUND_ITEM);
            }

            var updated = await _productRepository.UpdateOneAsync(
                product.Id,
                x => x.Set(p => p.Stock, request.Stock).Set(x => x.UpdateAt, DateTime.UtcNow),
                cancellationToken: cancellationToken);

            if (updated.AnyDocumentModified())
            {
                return ResponseApi<Unit>.ResponseOk(Unit.Value, "Cập nhập số lượng thành công");
            }
            
            return ResponseApi<Unit>.ResponseFail(StatusCodes.Status500InternalServerError, ResponseConstants.ERROR_EXECUTING);
        }
    }
}