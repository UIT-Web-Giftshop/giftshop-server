using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;

namespace Infrastructure.Interfaces.Repositories
{
    public interface ICounterRepository : IRepository<CounterCollection>
    {
        Task IncreaseAsync<TCollection>(int value = 1, CancellationToken cancellationToken = default) 
            where TCollection : class;
        
        Task DecreaseAsync<TCollection>(int value = -1, CancellationToken cancellationToken = default) 
            where TCollection : class;
    }
}