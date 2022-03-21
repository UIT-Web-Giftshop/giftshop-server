using Application.Features.Handlers;
using Domain.Entities;
using Infrastructure.Interfaces.Repositories;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using MediatR;

namespace Application.Features.Objects.Commands.Delete.DeleteListObjects
{
    public abstract class DeleteListObjectsHandler<T> : HandlerOfOneObject<T>, 
        IRequestHandler<DeleteListObjectsCommand<T>, ResponseApi<Unit>> where T : IdentifiableObject
    {
        public DeleteListObjectsHandler(IBaseRepository<T> _baseRepository) : base(_baseRepository, null)
        {

        }

        public async Task<ResponseApi<Unit>> Handle(DeleteListObjectsCommand<T> request, 
            CancellationToken cancellationToken)
        {
            var result = await this._baseRepository.DeleteManyAsync(p => p.Id, request.Ids, 
                cancellationToken);
            return result ? ResponseApi<Unit>.ResponseOk(Unit.Value, "Success") : 
                ResponseApi<Unit>.ResponseFail("Delete list products fail");
        }
    }
}