using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;

namespace Infrastructure.Interfaces.Repositories
{
    public interface ISaveFlagRepository : IBaseRepository<CountCollection>
    {
        Task AutoIncrementFlag<TCollection>(CancellationToken cancellationToken = default) 
            where TCollection : class;
        
        Task AutoDecrementFlag<TCollection>(CancellationToken cancellationToken = default) 
            where TCollection : class;
    }
}