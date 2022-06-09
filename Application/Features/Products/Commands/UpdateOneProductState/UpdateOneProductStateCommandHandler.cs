using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Infrastructure.Interfaces.Repositories;
using MediatR;
using MongoDB.Driver;

namespace Application.Features.Products.Commands.UpdateOneProductState
{
    public class UpdateOneProductStateCommandHandler : IRequestHandler<UpdateOneProductStateCommand, ResponseApi<Unit>>
    {
        private readonly IProductRepository _productRepository;

        public UpdateOneProductStateCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ResponseApi<Unit>> Handle(UpdateOneProductStateCommand request, CancellationToken cancellationToken)
        {
            var state = request.State == ProductState.Active;
            var updated = await _productRepository.FindOneAndUpdateAsync(
                x => x.Sku == request.Sku,
                x => x.Set(p => p.IsActive, state).Set(p => p.UpdateAt, DateTime.UtcNow),
                x => x.Id, 
                cancellationToken: cancellationToken);

            if (!string.IsNullOrEmpty(updated))
            {
                return ResponseApi<Unit>.ResponseOk(Unit.Value, "Cập nhập trạng thái thành công");
            }
            
            return ResponseApi<Unit>.ResponseFail(ResponseConstants.ERROR_NOT_FOUND_ITEM);
        }
    }
}