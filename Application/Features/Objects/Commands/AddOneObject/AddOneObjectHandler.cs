using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Objects.Commands.AddOneObject
{
    public abstract class AddOneObjectHandler<T, V> : Handler<T>, 
        IRequestHandler<AddOneObjectCommand<V>, ResponseApi<string>> where T : IdentifiableObject
    {
        public AddOneObjectHandler(IBaseRepository<T> _baseRepository, IMapper _mapper) : 
            base(_baseRepository, _mapper)
        {

        }

        public virtual T AddNewValue(T entity)
        {
            return entity;
        }

        public async Task<ResponseApi<string>> Handle(AddOneObjectCommand<V> request, 
            CancellationToken cancellationToken)
        {
            var entity = this._mapper.Map<T>(request.Data);
            entity = AddNewValue(entity);

            // Repository action
            var result = await this._baseRepository.AddAsync(entity, cancellationToken);
            
            return result == null ? ResponseApi<string>.ResponseFail(ResponseConstants.ERROR_EXECUTING)
                : ResponseApi<string>.ResponseOk(result.Id, "Success");
        }
    }
}