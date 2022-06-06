using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Infrastructure.Interfaces.Repositories
{
    public interface IRefactorRepository<TEntity> : IDisposable where TEntity : class
    {
        #region Queryable

        IMongoQueryable<TEntity> Query(AggregateOptions options = null);
        IMongoQueryable<TDerived> Query<TDerived>(AggregateOptions options = null) where TDerived : class, TEntity;

        #endregion
        
        #region Insert

        Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task InsertManyAsync(ICollection<TEntity> entities, CancellationToken cancellationToken = default);
        Task InsertManyAsync<TDerived>(ICollection<TDerived> entities, CancellationToken cancellationToken = default) where TDerived : class, TEntity;

        #endregion
        
        #region Get

        Task<TEntity> GetOneAsync(string id, CancellationToken cancellationToken = default);
        Task<TDerived> GetOneAsync<TDerived>(string id, CancellationToken cancellationToken = default) where TDerived : TEntity;
        Task<TReturnProjection> GetOneAsync<TReturnProjection>(
            string id,
            Expression<Func<TEntity, TReturnProjection>> returnProjection,
            CancellationToken cancellationToken = default);

        IFindFluent<TEntity, TEntity> GetAll(FindOptions options = null);

        #endregion

        #region Find fluent

        IFindFluent<TEntity, TEntity> FindFluent(
            Expression<Func<TEntity, bool>> filter, 
            FindOptions options = null);
        IFindFluent<TDerived, TDerived> FindFluent<TDerived>(
            Expression<Func<TDerived, bool>> filter,
            FindOptions options = null) where TDerived : TEntity;
        
        IFindFluent<TEntity, TEntity> FindFluent(
            Expression<Func<TEntity, object>> property,
            string regexPattern,
            string regexOptions = "i",
            FindOptions options = null);
        IFindFluent<TEntity, TEntity> FindFluent(
            FieldDefinition<TEntity> property, 
            string regexPattern, 
            string regexOptions = "i", 
            FindOptions options = null);
        IFindFluent<TEntity, TEntity> FindFluent(
            IEnumerable<Expression<Func<TEntity, object>>> properties,
            string regexPattern,
            string regexOptions = "i",
            FindOptions options = null);
        IFindFluent<TEntity, TEntity> FindFluent(
            IEnumerable<FieldDefinition<TEntity>> properties,
            string regexPattern,
            string regexOptions = "i",
            FindOptions options = null);
        
        IFindFluent<TDerived, TDerived> FindFluent<TDerived>(
            Expression<Func<TDerived, object>> property,
            string regexPattern,
            string regexOptions = "i",
            FindOptions options = null) where TDerived : TEntity;
        IFindFluent<TDerived, TDerived> FindFluent<TDerived>(
            FieldDefinition<TDerived> property,
            string regexPattern,
            string regexOptions = "i",
            FindOptions options = null) where TDerived : TEntity;
        IFindFluent<TDerived, TDerived> FindFluent<TDerived>(
            IEnumerable<Expression<Func<TDerived, object>>> properties,
            string regexPattern,
            string regexOptions = "i",
            FindOptions options = null) where TDerived : TEntity;
        IFindFluent<TDerived, TDerived> FindFluent<TDerived>(
            IEnumerable<FieldDefinition<TDerived>> properties,
            string regexPattern,
            string regexOptions = "i",
            FindOptions options = null) where TDerived : TEntity;

        Task<IAsyncCursor<TEntity>> FindAsync(
            Expression<Func<TEntity, bool>> filter,
            FindOptions<TEntity, TEntity> options = null, 
            CancellationToken cancellationToken = default);
        Task<IAsyncCursor<TDerived>> FindAsync<TDerived>(
            Expression<Func<TDerived, bool>> filter,
            FindOptions<TDerived, TDerived> options = null,
            CancellationToken cancellationToken = default) where TDerived : TEntity;
        Task<IAsyncCursor<TReturnProjection>> FindAsync<TReturnProjection>(
            Expression<Func<TEntity, bool>> filter,
            Expression<Func<TEntity, TReturnProjection>> returnProjection,
            FindOptions<TEntity, TReturnProjection> options = null,
            CancellationToken cancellationToken = default);
        Task<IAsyncCursor<TReturnProject>> FindAsync<TDerived, TReturnProject>(
            Expression<Func<TDerived, bool>> filter,
            Expression<Func<TDerived, TReturnProject>> returnProjection,
            FindOptions<TDerived, TReturnProject> options = null,
            CancellationToken cancellationToken = default) where TDerived : TEntity;

        Task<TEntity> FindOneAsync(
            Expression<Func<TEntity, bool>> filter,
            CancellationToken cancellationToken = default);
        Task<TDerived> FindOneAsync<TDerived>(
            Expression<Func<TDerived, bool>> filter,
            CancellationToken cancellationToken = default) where TDerived : TEntity;
        Task<TReturnProject> FindOneAsync<TReturnProject>(
            Expression<Func<TEntity, bool>> filter,
            Expression<Func<TEntity, TReturnProject>> returnProjection,
            CancellationToken cancellationToken = default);
        Task<TReturnProject> FindOneAsync<TDerived, TReturnProject>(
            Expression<Func<TDerived, bool>> filter,
            Expression<Func<TDerived, TReturnProject>> returnProjection,
            CancellationToken cancellationToken = default) where TDerived : TEntity;

        IFindFluent<TEntity, TEntity> FindFluent(
            FilterDefinition<TEntity> filter, 
            FindOptions options = null);
        IFindFluent<TDerived, TDerived> FindFluent<TDerived>(
            FilterDefinition<TDerived> filter,
            FindOptions options = null) where TDerived : TEntity;

        #endregion
        
        IAggregateFluent<TEntity> Aggregate(AggregateOptions options = null);

        #region Update

        Task<UpdateResult> UpdateOneAsync(
            string id,
            Func<UpdateDefinitionBuilder<TEntity>, UpdateDefinition<TEntity>> update,
            bool isUpsert = false,
            CancellationToken cancellationToken = default);
        Task<UpdateResult> UpdateOneAsync(
            string id,
            Func<UpdateDefinitionBuilder<TEntity>, UpdateDefinition<TEntity>> update,
            UpdateOptions options,
            CancellationToken cancellationToken = default);

        Task<UpdateResult> UpdateOneAsync(
            Expression<Func<TEntity, bool>> filter,
            Func<UpdateDefinitionBuilder<TEntity>, UpdateDefinition<TEntity>> update,
            bool isUpsert = false,
            CancellationToken cancellationToken = default);
        Task<UpdateResult> UpdateOneAsync(
            FilterDefinition<TEntity> filter,
            Func<UpdateDefinitionBuilder<TEntity>, UpdateDefinition<TEntity>> update,
            bool isUpsert = false,
            CancellationToken cancellationToken = default);
        
        // base operations
        Task<UpdateResult> UpdateOneAsync(
            Expression<Func<TEntity, bool>> filter,
            Func<UpdateDefinitionBuilder<TEntity>, UpdateDefinition<TEntity>> update,
            UpdateOptions options,
            CancellationToken cancellationToken = default);
        Task<UpdateResult> UpdateOneAsync(
            FilterDefinition<TEntity> filter,
            Func<UpdateDefinitionBuilder<TEntity>, UpdateDefinition<TEntity>> update,
            UpdateOptions options,
            CancellationToken cancellationToken = default);
        
        // derived
        Task<UpdateResult> UpdateOneAsync<TDerived>(
            string id,
            Func<UpdateDefinitionBuilder<TDerived>, UpdateDefinition<TDerived>> update,
            bool isUpsert = false,
            CancellationToken cancellationToken = default) where TDerived : TEntity;
        Task<UpdateResult> UpdateOneAsync<TDerived>(
            string id,
            Func<UpdateDefinitionBuilder<TDerived>, UpdateDefinition<TDerived>> update,
            UpdateOptions options,
            CancellationToken cancellationToken = default) where TDerived : TEntity;

        Task<UpdateResult> UpdateOneAsync<TDerived>(
            Expression<Func<TDerived, bool>> filter,
            Func<UpdateDefinitionBuilder<TDerived>, UpdateDefinition<TDerived>> update,
            bool isUpsert = false,
            CancellationToken cancellationToken = default) where TDerived : TEntity;
        Task<UpdateResult> UpdateOneAsync<TDerived>(
            FilterDefinition<TDerived> filter,
            Func<UpdateDefinitionBuilder<TDerived>, UpdateDefinition<TDerived>> update,
            bool isUpsert = false,
            CancellationToken cancellationToken = default) where TDerived : TEntity;
        Task<UpdateResult> UpdateOneAsync<TDerived>(
            Expression<Func<TDerived, bool>> filter,
            Func<UpdateDefinitionBuilder<TDerived>, UpdateDefinition<TDerived>> update,
            UpdateOptions options,
            CancellationToken cancellationToken = default) where TDerived : TEntity;
        Task<UpdateResult> UpdateOneAsync<TDerived>(
            FilterDefinition<TDerived> filter,
            Func<UpdateDefinitionBuilder<TDerived>, UpdateDefinition<TDerived>> update,
            UpdateOptions options,
            CancellationToken cancellationToken = default) where TDerived : TEntity;

        
        // find and update
        Task<TProjection> FindOneAndUpdateAsync<TProjection>(
            Expression<Func<TEntity, bool>> filter,
            Func<UpdateDefinitionBuilder<TEntity>, UpdateDefinition<TEntity>> update,
            Expression<Func<TEntity, TProjection>> returnProjection,
            ReturnDocument returnDocument = ReturnDocument.After,
            bool isUpsert = false,
            CancellationToken cancellationToken = default);
        Task<TProjection> FindOneAndUpdateAsync<TProjection>(
            Expression<Func<TEntity, bool>> filter,
            Func<UpdateDefinitionBuilder<TEntity>, UpdateDefinition<TEntity>> update,
            ProjectionDefinition<TEntity, TProjection> returnProjection,
            ReturnDocument returnDocument= ReturnDocument.After,
            bool isUpsert = false,
            CancellationToken cancellationToken = default);

        #endregion

    }
}