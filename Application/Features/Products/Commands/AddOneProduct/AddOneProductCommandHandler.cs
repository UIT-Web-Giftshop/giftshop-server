using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Products.Commands.AddOneProduct
{
    public class AddOneProductCommandHandler : IRequestHandler<AddOneProductCommand, ResponseApi<string>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public AddOneProductCommandHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ResponseApi<string>> Handle(AddOneProductCommand request, CancellationToken cancellationToken)
        {
            var existedProduct = await _productRepository.FindOneAsync(x => x.Sku == request.Product.Sku, cancellationToken);

            if (existedProduct is not null)
            {
                return ResponseApi<string>.ResponseFail("Sản phẩm đã tồn tại");
            }
            
            var entity = _mapper.Map<Product>(request.Product);
            entity.CreatedAt = DateTime.Now;
            

            // Repository action
            try
            {
                await _productRepository.InsertAsync(entity, cancellationToken);
                return ResponseApi<string>.ResponseOk(entity.Sku, StatusCodes.Status201Created,
                    "Thêm sản phẩm mới thành công");
            }
            catch (Exception)
            {
                return ResponseApi<string>.ResponseFail(ResponseConstants.ERROR_EXECUTING);
            }
        }
    }
}