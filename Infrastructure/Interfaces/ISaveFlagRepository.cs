using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;

namespace Infrastructure.Interfaces
{
    public interface ISaveFlagRepository : IBaseRepository<SaveFlag>
    {
        Task AutoIncrementFlag<TCollection>(CancellationToken cancellationToken = default) 
            where TCollection : class;
    }
}