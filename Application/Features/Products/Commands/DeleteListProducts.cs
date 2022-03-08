using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.Features.Products.Commands
{
    public class DeleteListProducts
    {
        public class Command : IRequest<ResponseApi<Unit>>
        {
            public List<string> Ids { get; init; }
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
                var result = await _productRepository.DeleteManyAsync(
                    p => p.Id,
                    request.Ids,
                    cancellationToken);

                return result 
                    ? ResponseApi<Unit>.ResponseOk(Unit.Value, "Delete list products success") 
                    : ResponseApi<Unit>.ResponseFail("Delete list products fail");
            }
        }
    }
}