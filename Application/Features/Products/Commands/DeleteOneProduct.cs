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
        public class Command : IRequest<ResponseApi<Unit>>
        {
            public string Id { get; init; }
        }
        
        public class Handler : IRequestHandler<Command, ResponseApi<Unit>>
        {
            private readonly IProductRepository _productRepository;

            public Handler(IProductRepository productRepository)
            {
                _productRepository = productRepository;
            }

            public async Task<ResponseApi<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var existedProduct = await _productRepository.GetOneAsync(p => p.Id == request.Id, cancellationToken);
                if (existedProduct == null)
                {
                    return ResponseApi<Unit>.ResponseFail(ResponseConstants.ERROR_NOT_FOUND_ITEM);
                }
                var result = await _productRepository.RemoveAsync(request.Id, cancellationToken);
                return result 
                    ? ResponseApi<Unit>.ResponseOk(Unit.Value, "Delete product success") 
                    : ResponseApi<Unit>.ResponseFail(StatusCodes.Status500InternalServerError, ResponseConstants.ERROR_EXECUTING);
            }
        }
    }
}