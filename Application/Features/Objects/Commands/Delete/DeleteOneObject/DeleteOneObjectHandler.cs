using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Application.Features.Handlers;
using Domain.Entities;
using Infrastructure.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Objects.Commands.Delete.DeleteOneObject
{
    public abstract class DeleteOneObjectHandler<T> : HandlerOfOneObject<T>, 
        IRequestHandler<DeleteOneObjectCommand<T>, ResponseApi<Unit>> where T : IdentifiableObject
    {
        public DeleteOneObjectHandler(IBaseRepository<T> _baseRepository) : base(_baseRepository, null)
        {
            
        }

        public async Task<ResponseApi<Unit>> Handle(DeleteOneObjectCommand<T> request, 
            CancellationToken cancellationToken)
        {
            Expression<Func<T, bool>> expression = p => p.Id == request.Id;
            var existedObject = await this._baseRepository.GetOneAsync(expression, cancellationToken);
            
            if (existedObject == null)
            {
                return ResponseApi<Unit>.ResponseFail(ResponseConstants.ERROR_NOT_FOUND_ITEM);
            }

            var result = await this._baseRepository.DeleteOneAsync(expression, cancellationToken);
            return result ? ResponseApi<Unit>.ResponseOk(Unit.Value, "Success") : 
                ResponseApi<Unit>.ResponseFail(StatusCodes.Status500InternalServerError, 
                ResponseConstants.ERROR_EXECUTING);
        }
    }
}