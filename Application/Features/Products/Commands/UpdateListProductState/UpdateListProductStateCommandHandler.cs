using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Application.Features.Products.Commands.UpdateOneProductState;
using Infrastructure.Interfaces.Repositories;
using MediatR;
using MongoDB.Driver;

namespace Application.Features.Products.Commands.UpdateListProductState
{
    public class UpdateListProductStateCommandHandler : IRequestHandler<UpdateListProductStateCommand, ResponseApi<Unit>>
    {
        private readonly IProductRepository _productRepository;

        public UpdateListProductStateCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ResponseApi<Unit>> Handle(UpdateListProductStateCommand request, CancellationToken cancellationToken)
        {
            var state = request.State == ProductState.Active;
            try
            {
                await _productRepository.UpdateManyAsync(
                    p => request.Skus.Contains(p.Sku),
                    x => x.Set(p => p.IsActive, state).Set(p => p.UpdateAt, DateTime.UtcNow),
                    cancellationToken: cancellationToken);
                return ResponseApi<Unit>.ResponseOk(Unit.Value, "Delete list products success");
            }
            catch (Exception)
            {
                return ResponseApi<Unit>.ResponseFail("Delete list products fail");
            }
        }
    }
}