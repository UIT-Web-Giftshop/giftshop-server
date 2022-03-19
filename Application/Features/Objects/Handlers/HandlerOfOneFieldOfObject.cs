using Infrastructure.Interfaces.Repositories;

namespace Application.Features.Objects.Handlers
{
    public abstract class HandlerOfOneFieldOfObject<T> : Handler<T> where T : class
    {
        public HandlerOfOneFieldOfObject(IBaseRepository<T> _baseRepository) : base(_baseRepository)
        {

        }
    }
}