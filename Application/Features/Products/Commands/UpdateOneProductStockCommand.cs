using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Infrastructure.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Products.Commands
{
    public class UpdateOneProductStockCommand : IRequest<ResponseApi<Unit>>
    {
        public string Id { get; init; }
        public uint Stock { get; init; }
    }
    
    public class UpdateProductStockCommandHandler : IRequestHandler<UpdateOneProductStockCommand, ResponseApi<Unit>>
    {
        private readonly IProductRepository _productRepository;

        public UpdateProductStockCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ResponseApi<Unit>> Handle(UpdateOneProductStockCommand request, CancellationToken cancellationToken)
        {
            var existedProduct = await _productRepository.GetOneAsync(q => q.Id == request.Id, cancellationToken);
            if (existedProduct == null)
            {
                return ResponseApi<Unit>.ResponseFail(ResponseConstants.ERROR_NOT_FOUND_ITEM);
            }

            var result = await _productRepository
                .PatchOneFieldAsync(q => q.Id == request.Id, p => p.Stock, request.Stock, cancellationToken);
                
            return result 
                ? ResponseApi<Unit>.ResponseOk(Unit.Value, "Update product's quantity success") 
                : ResponseApi<Unit>.ResponseFail(StatusCodes.Status500InternalServerError, ResponseConstants.ERROR_EXECUTING);
        }
    }
}