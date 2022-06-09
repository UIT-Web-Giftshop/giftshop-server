using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using AutoMapper;
using Domain.ViewModels.Product;
using Infrastructure.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Products.Queries.GetOneProductBySku
{
    public class GetOneProductBySkuQueryHandler : IRequestHandler<GetOneProductBySkuQuery, ResponseApi<ProductDetailViewModel>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        
        public GetOneProductBySkuQueryHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }
        
        public async Task<ResponseApi<ProductDetailViewModel>> Handle(GetOneProductBySkuQuery request, CancellationToken cancellationToken)
        {
            var data = await _productRepository.FindOneAsync(q => q.Sku == request.Sku, cancellationToken);
            
            if (data is null)
                return ResponseApi<ProductDetailViewModel>.ResponseFail(ResponseConstants.ERROR_NOT_FOUND_ITEM);
            
            return ResponseApi<ProductDetailViewModel>.ResponseOk(_mapper.Map<ProductDetailViewModel>(data));
        }
    }
}