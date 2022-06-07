using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Application.Features.Products.Commands.UpdateOneProductDetail;
using AutoMapper;
using Infrastructure.Extensions.Mongo;
using Infrastructure.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Products.Commands
{
    public class UpdateOneProductInfoCommandHandler : IRequestHandler<UpdateOneProductDetailCommand, ResponseApi<Unit>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public UpdateOneProductInfoCommandHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ResponseApi<Unit>> Handle(UpdateOneProductDetailCommand request,
            CancellationToken cancellationToken)
        {
            var existedProduct =
                await _productRepository.FindOneAsync(x => x.Sku == request.Product.Sku, cancellationToken);

            if (existedProduct is null)
            {
                return ResponseApi<Unit>.ResponseFail(ResponseConstants.ERROR_NOT_FOUND_ITEM);
            }
            
            var newProduct = _mapper.Map(request.Product, existedProduct);
            newProduct.UpdateAt = DateTime.UtcNow;

            var replaced = await _productRepository.ReplaceOneAsync(
                newProduct.Id,
                newProduct,
                cancellationToken: cancellationToken);
            

            if (replaced.AnyDocumentReplaced())
            {
                return ResponseApi<Unit>.ResponseOk(Unit.Value, "Cập nhập sản phẩm thành công");
            }

            return ResponseApi<Unit>.ResponseFail(StatusCodes.Status500InternalServerError,
                ResponseConstants.ERROR_EXECUTING);
        }
    }
}