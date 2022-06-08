using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Domain.Paging;

namespace Infrastructure.Interfaces.Repositories
{
    [Obsolete("Use IRepository<TEntity> instead")]
    public interface IBaseRepositoryLegacy<T> where T : class
    {
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
        
        Task<T> GetOneAsync(
            Expression<Func<T, bool>> expression, 
            CancellationToken cancellationToken = default);
        
        Task<IEnumerable<T>> GetPagingAsync(
            PagingRequest pagingRequest,
            Expression<Func<T, bool>> expression = null,
            Expression<Func<T, object>> sortBy = null,
            bool sortAscending = true,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// ONLY FOR TESTING, NOT TO USE
        /// </summary>
        Task<IEnumerable<T>> GetManyAsync(
            Expression<Func<T, bool>> expression = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            CancellationToken cancellationToken = default);
        
        Task<bool> UpdateAsync(
            Expression<Func<T, bool>> fieldName,
            T entity, 
            CancellationToken cancellationToken = default);
        
        Task<bool> PatchOneFieldAsync<TValue>(
            Expression<Func<T, bool>> expression, 
            Expression<Func<T, TValue>> fieldName, 
            TValue fieldValue, 
            CancellationToken cancellationToken = default);
        
        Task<bool> DeleteOneAsync(
            Expression<Func<T, bool>> expression, 
            CancellationToken cancellationToken = default);
        
        Task<bool> DeleteManyAsync<TValue>(
            Expression<Func<T, TValue>> expression,
            IEnumerable<TValue> values,
            CancellationToken cancellationToken = default);
    }
}