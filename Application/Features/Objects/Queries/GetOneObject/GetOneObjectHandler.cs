using AutoMapper;
using Infrastructure.Interfaces.Repositories;

namespace Application.Features.Objects.Queries.GetOneObject
{
    public abstract class GetOneObjectHandler<T> where T : class
    {
        protected readonly IBaseRepository<T> _baseRepository;
        protected readonly IMapper _mapper;

        public GetOneObjectHandler(IBaseRepository<T> _baseRepository, IMapper _mapper)
        {
            this._baseRepository = _baseRepository;
            this._mapper = _mapper;
        }
    }
}