using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Application.Features.Handlers;
using Application.Features.Objects.Vms;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Objects.Commands.Update.UpdateOneObject
{
    public abstract class UpdateOneObjectInfoHandler<T, V> : HandlerOfOneObject<T>, 
        IRequestHandler<UpdateOneObjectCommand<V>, ResponseApi<Unit>> where T : IdentifiableObject
        where V : ObjectVm
    {
        public UpdateOneObjectInfoHandler(IBaseRepository<T> _baseRepository, IMapper _mapper) : 
            base(_baseRepository, _mapper)
        {

        }

        public async Task<ResponseApi<Unit>> Handle(UpdateOneObjectCommand<V> request, 
            CancellationToken cancellationToken)
        {
            Expression<Func<T, bool>> expression = p => p.Id == request.Id;
            var existedData = await this._baseRepository.GetOneAsync(expression, cancellationToken);

            if (existedData == null)
            {
                return ResponseApi<Unit>.ResponseFail(ResponseConstants.ERROR_NOT_FOUND_ITEM);
            }

            this._mapper.Map<V, T>(request.Data, existedData);
            existedData.Update();

            var result = await this._baseRepository.UpdateAsync(expression, existedData, 
                cancellationToken);
            
            return result ? ResponseApi<Unit>.ResponseOk(Unit.Value, "Success") : 
                ResponseApi<Unit>.ResponseFail(ResponseConstants.ERROR_EXECUTING);
        }
    }
}