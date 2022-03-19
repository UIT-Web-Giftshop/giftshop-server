using Infrastructure.Interfaces.Repositories;

namespace Application.Features.Objects.Handlers
{
    public abstract class Handler<T> where T : class
    {
        protected readonly IBaseRepository<T> _baseRepository;

        public Handler(IBaseRepository<T> _baseRepository)
        {
            this._baseRepository = _baseRepository;
        }
    }
}