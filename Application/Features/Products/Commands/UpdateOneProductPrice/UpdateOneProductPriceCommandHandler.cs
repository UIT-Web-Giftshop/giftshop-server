using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Infrastructure.Extensions.Mongo;
using Infrastructure.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;

namespace Application.Features.Products.Commands.UpdateOneProductPrice
{
    
    
    public class UpdateOneProductPriceCommandHandler : IRequestHandler<UpdateOneProductPriceCommand, ResponseApi<Unit>>
    {
        private readonly IProductRepository _productRepository;

        public UpdateOneProductPriceCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ResponseApi<Unit>> Handle(UpdateOneProductPriceCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.FindOneAsync(x => x.Sku == request.Sku, cancellationToken);
            if (product is null)
            {
                return ResponseApi<Unit>.ResponseFail(ResponseConstants.ERROR_NOT_FOUND_ITEM);
            }

            var updated = await _productRepository.UpdateOneAsync(
                product.Id,
                x => x.Set(p => p.Price, request.Price).Set(x => x.UpdateAt, DateTime.UtcNow),
                cancellationToken: cancellationToken);

            if (updated.AnyDocumentModified())
            {
                return ResponseApi<Unit>.ResponseOk(Unit.Value, "Chỉnh sửa giá thành công");
            }
            
            return ResponseApi<Unit>.ResponseFail(StatusCodes.Status500InternalServerError, ResponseConstants.ERROR_EXECUTING);
        }
    }
}