using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Application.Features.Objects.Handlers;
using Domain.Entities;
using Infrastructure.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Objects.Commands.Update.UpdateOneFiledOfObject
{
    public abstract class UpdateOneFieldOfObjectHandler<T, V> : HandlerOfOneFieldOfObject<T>,
        IRequestHandler<V, ResponseApi<Unit>> where T : IdentifiableObject where V : 
        UpdateOneFieldOfObjectCommand
    {
        public UpdateOneFieldOfObjectHandler(IBaseRepository<T> _baseRepository) : base(_baseRepository)
        {

        }

        public virtual async Task<bool> GetResult(Expression<Func<T, bool>> expression, 
            UpdateOneFieldOfObjectCommand request, CancellationToken cancellationToken)
        {
            return await TaskEx.FromResult(false);
        }

        public async Task<ResponseApi<Unit>> Handle(V request, CancellationToken cancellationToken)
        {
            Expression<Func<T, bool>> expression = p => p.Id == request.Id;
            var existedData = await this._baseRepository.GetOneAsync(expression, cancellationToken);

            if (existedData == null)
            {
                return ResponseApi<Unit>.ResponseFail(ResponseConstants.ERROR_NOT_FOUND_ITEM);
            }

            var result = await GetResult(expression, request, cancellationToken);
            return result ? ResponseApi<Unit>.ResponseOk(Unit.Value, "Success") : 
                ResponseApi<Unit>.ResponseFail(StatusCodes.Status500InternalServerError, 
                ResponseConstants.ERROR_EXECUTING);
        }
    }
}