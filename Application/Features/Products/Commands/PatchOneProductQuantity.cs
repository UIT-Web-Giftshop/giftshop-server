using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Products.Commands
{
    public class PatchOneProductQuantity
    {
        public class Command : IRequest<ResponseApi<Unit>>
        {
            public string Id { get; init; }
            public uint Quantity { get; init; }
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
                var existedProduct = await _productRepository.GetOneAsync(q => q.Id == request.Id, cancellationToken);
                if (existedProduct == null)
                {
                    return ResponseApi<Unit>.ResponseFail(ResponseConstants.ERROR_NOT_FOUND_ITEM);
                }

                existedProduct.Quantity = request.Quantity;
                
                var result = await _productRepository
                    .PatchOneFieldAsync(q => q.Id == request.Id, p => p.Quantity, request.Quantity, cancellationToken);
                
                return result 
                    ? ResponseApi<Unit>.ResponseOk(Unit.Value, "Update product's quantity success") 
                    : ResponseApi<Unit>.ResponseFail(StatusCodes.Status500InternalServerError, ResponseConstants.ERROR_EXECUTING);
            }
        }
    }
}