using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Domain.Paging;

namespace Infrastructure.Interfaces
{
    public interface IBaseRepository<T> : IDisposable where T : class
    {
        /// <summary>
        /// Add one document to collection
        /// </summary>
        /// <param name="entity">Type of document</param>
        /// <param name="cancellationToken">default</param>
        /// <returns></returns>
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Get one document from collection
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T> GetOneAsync(
            Expression<Func<T, bool>> expression, 
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Get documents and paging from collection
        /// </summary>
        /// <param name="pagingRequest"></param>
        /// <param name="expression"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortAscending"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetPagingAsync(
            PagingRequest pagingRequest,
            Expression<Func<T, bool>> expression = null,
            string sortBy = null,
            bool sortAscending = true,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// ONLY FOR TESTING, NOT TO USE
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="orderBy"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetManyAsync(
            Expression<Func<T, bool>> expression = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Update entire document in collection
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(
            Expression<Func<T, bool>> fieldName,
            T entity, 
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Just update one field in document
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        Task<bool> PatchOneFieldAsync<TValue>(
            Expression<Func<T, bool>> expression, 
            Expression<Func<T, TValue>> fieldName, 
            TValue fieldValue, 
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Delete one document from collection
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> DeleteOneAsync(
            Expression<Func<T, bool>> expression, 
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Delete many documents from collection
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="values"></param>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        Task<bool> DeleteManyAsync<TValue>(
            Expression<Func<T, TValue>> expression,
            IEnumerable<TValue> values,
            CancellationToken cancellationToken = default);
    }
}