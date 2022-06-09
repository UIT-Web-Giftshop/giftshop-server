using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Domain.Entities;
using Infrastructure.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Products.Queries.GetOneProductById
{
    public class GetOneProductByIdQueryHandler : IRequestHandler<GetOneProductByIdQuery, ResponseApi<Product>>
    {
        private readonly IProductRepository _productRepository;

        public GetOneProductByIdQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ResponseApi<Product>> Handle(GetOneProductByIdQuery request, CancellationToken cancellationToken)
        {
            var data = await _productRepository.GetOneAsync(request.Id, cancellationToken);
            return ResponseApi<Product>.ResponseOk(data);
        }
    }
}