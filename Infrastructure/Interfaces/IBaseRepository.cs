using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IBaseRepository<T> : IDisposable where T : class
    {
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
        Task<T> GetByIdAsync(string id);
        Task<IEnumerable<T>> GetManyAsync(
            Expression<Func<T, bool>> expression = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken = default);
        Task<bool> RemoveAsync(string id, CancellationToken cancellationToken = default);
        Task<bool> RemoveManyAsync(
            Expression<Func<T, bool>> expression,
            CancellationToken cancellationToken = default);
    }
}