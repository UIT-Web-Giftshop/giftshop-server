using Application.Features.Objects.Handlers;
using AutoMapper;
using Infrastructure.Interfaces.Repositories;

namespace Application.Features.Handlers
{
    public abstract class HandlerOfOneObject<T> : Handler<T> where T : class
    {
        protected readonly IMapper _mapper;

        public HandlerOfOneObject(IBaseRepository<T> _baseRepository, IMapper _mapper) : 
            base(_baseRepository)
        {
            this._mapper = _mapper;
        }
    }
}