using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Products.Commands
{
    public class DeleteOneProduct
    {
        public class Command : IRequest<ResponseApi<bool>>
        {
            public string Id { get; init; }
        }
        
        public class Handler : IRequestHandler<Command, ResponseApi<bool>>
        {
            private readonly IProductRepository _productRepository;

            public Handler(IProductRepository productRepository)
            {
                _productRepository = productRepository;
            }

            public async Task<ResponseApi<bool>> Handle(Command request, CancellationToken cancellationToken)
            {
                var existedProduct = await _productRepository.GetOneAsync(p => p.Id == request.Id, cancellationToken);
                if (existedProduct == null)
                {
                    return ResponseApi<bool>.ResponseFail(ResponseConstants.ERROR_NOT_FOUND_ITEM);
                }
                var result = await _productRepository.RemoveAsync(request.Id, cancellationToken);
                return result 
                    ? ResponseApi<bool>.ResponseOk(true, "Delete product success") 
                    : ResponseApi<bool>.ResponseFail(StatusCodes.Status500InternalServerError, ResponseConstants.ERROR_EXECUTING);
            }
        }
    }
}