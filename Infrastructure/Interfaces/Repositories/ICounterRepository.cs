using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;

namespace Infrastructure.Interfaces.Repositories
{
    public interface ICounterRepository : IRefactorRepository<CounterCollection>
    {
        Task IncreaseCounter<TCollection>(CancellationToken cancellationToken = default) 
            where TCollection : class;
        
        Task DecreaseCounter<TCollection>(CancellationToken cancellationToken = default) 
            where TCollection : class;
    }
}