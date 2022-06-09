using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Context;
using Infrastructure.Interfaces.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Infrastructure.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly IMongoCollection<TEntity> _mongoCollection;
        protected readonly IMongoContext _mongoContext;

        internal IMongoCollection<TEntity> Collection => _mongoCollection;

        public Repository(IMongoContext mongoContext)
        {
            _mongoContext = mongoContext;
            _mongoCollection = _mongoContext.GetCollection<TEntity>();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #region Query

        public virtual IMongoQueryable<TEntity> Query(AggregateOptions options = null)
        {
            return _mongoCollection.AsQueryable(options);
        }

        public virtual IMongoQueryable<TDerived> Query<TDerived>(AggregateOptions options = null)
            where TDerived : class, TEntity
        {
            return _mongoCollection.OfType<TDerived>().AsQueryable(options);
        }

        #endregion

        #region Insert

        public virtual async Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await _mongoCollection.InsertOneAsync(entity, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public virtual async Task InsertManyAsync(ICollection<TEntity> entities,
            CancellationToken cancellationToken = default)
        {
            var models = new List<WriteModel<TEntity>>(entities.Count);
            foreach (var item in entities)
            {
                var upsert = new InsertOneModel<TEntity>(item);
                models.Add(upsert);
            }


            await _mongoCollection.InsertManyAsync(entities, new InsertManyOptions(){IsOrdered = false});
        }


        public virtual async Task InsertManyAsync<TDerived>(ICollection<TDerived> entities,
            CancellationToken cancellationToken = default) where TDerived : class, TEntity
        {
            if (entities.Count > 0)
            {
                await _mongoCollection
                    .OfType<TDerived>()
                    .InsertManyAsync(entities, cancellationToken: cancellationToken)
                    .ConfigureAwait(false);
            }
        }

        #endregion

        #region Get

        public virtual async Task<TEntity> GetOneAsync(string id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));

            var filter = new BsonDocument("_id", ObjectId.Parse(id));
            var cursor = await _mongoCollection.FindAsync(filter, null, cancellationToken)
                .ConfigureAwait(false);

            return await cursor.FirstOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public virtual async Task<TDerived> GetOneAsync<TDerived>(string id,
            CancellationToken cancellationToken = default)
            where TDerived : TEntity
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));

            var filter = new BsonDocument("_id", ObjectId.Parse(id));
            var cursor = await _mongoCollection.OfType<TDerived>()
                .FindAsync(filter, cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            return await cursor.FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task<TReturnProjection> GetOneAsync<TReturnProjection>(string id,
            Expression<Func<TEntity, TReturnProjection>> returnProjection,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));

            var filter = new BsonDocument("_id", ObjectId.Parse(id));
            var cursor = await _mongoCollection.FindAsync(
                    filter,
                    new FindOptions<TEntity, TReturnProjection>()
                    {
                        Projection = Builders<TEntity>.Projection.Expression(returnProjection)
                    },
                    cancellationToken)
                .ConfigureAwait(false);

            return await cursor.FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual IFindFluent<TEntity, TEntity> GetAll(FindOptions options = null)
        {
            return _mongoCollection.Find(FilterDefinition<TEntity>.Empty, options);
        }

        #endregion

        #region Find

        public virtual IFindFluent<TEntity, TEntity> FindFluent(
            Expression<Func<TEntity, bool>> filter,
            FindOptions options = null)
        {
            return _mongoCollection.Find(filter, options);
        }

        public virtual IFindFluent<TDerived, TDerived> FindFluent<TDerived>(
            Expression<Func<TDerived, bool>> filter,
            FindOptions options = null) where TDerived : TEntity
        {
            return _mongoCollection.OfType<TDerived>()
                .Find(filter, options);
        }

        public virtual IFindFluent<TEntity, TEntity> FindFluent(
            Expression<Func<TEntity, object>> property,
            string regexPattern,
            string regexOptions = "i",
            FindOptions options = null)
        {
            return _mongoCollection
                .Find(
                    Builders<TEntity>.Filter.Regex(property, new BsonRegularExpression(regexPattern, regexOptions)),
                    options);
        }

        public virtual IFindFluent<TEntity, TEntity> FindFluent(
            FieldDefinition<TEntity> property,
            string regexPattern,
            string regexOptions = "i",
            FindOptions options = null)
        {
            return _mongoCollection
                .Find(Builders<TEntity>.Filter.Regex(property, new BsonRegularExpression(regexPattern, regexOptions)),
                    options);
        }

        public virtual IFindFluent<TEntity, TEntity> FindFluent(
            IEnumerable<Expression<Func<TEntity, object>>> properties,
            string regexPattern,
            string regexOptions = "i",
            FindOptions options = null)
        {
            var filters = properties.Select(p =>
                Builders<TEntity>.Filter.Regex(p, new BsonRegularExpression(regexPattern, regexOptions)));

            return _mongoCollection.Find(Builders<TEntity>.Filter.Or(filters), options);
        }

        public virtual IFindFluent<TEntity, TEntity> FindFluent(
            IEnumerable<FieldDefinition<TEntity>> properties,
            string regexPattern,
            string regexOptions = "i",
            FindOptions options = null)
        {
            var filters = properties.Select(p =>
                Builders<TEntity>.Filter.Regex(p, new BsonRegularExpression(regexPattern, regexOptions)));
            return _mongoCollection.Find(Builders<TEntity>.Filter.Or(filters), options);
        }

        public virtual IFindFluent<TDerived, TDerived> FindFluent<TDerived>(
            Expression<Func<TDerived, object>> property,
            string regexPattern,
            string regexOptions = "i",
            FindOptions options = null) where TDerived : TEntity
        {
            return _mongoCollection.OfType<TDerived>()
                .Find(Builders<TDerived>.Filter.Regex(property, new BsonRegularExpression(regexPattern, regexOptions)),
                    options);
        }

        public virtual IFindFluent<TDerived, TDerived> FindFluent<TDerived>(
            FieldDefinition<TDerived> property,
            string regexPattern,
            string regexOptions = "i",
            FindOptions options = null) where TDerived : TEntity
        {
            return _mongoCollection.OfType<TDerived>()
                .Find(Builders<TDerived>.Filter.Regex(property, new BsonRegularExpression(regexPattern, regexOptions)),
                    options);
        }

        public virtual IFindFluent<TDerived, TDerived> FindFluent<TDerived>(
            IEnumerable<Expression<Func<TDerived, object>>> properties,
            string regexPattern,
            string regexOptions = "i",
            FindOptions options = null) where TDerived : TEntity
        {
            var filters = properties.Select(p =>
                Builders<TDerived>.Filter.Regex(p, new BsonRegularExpression(regexPattern, regexOptions)));
            return _mongoCollection.OfType<TDerived>()
                .Find(Builders<TDerived>.Filter.Or(filters), options);
        }

        public virtual IFindFluent<TDerived, TDerived> FindFluent<TDerived>(
            IEnumerable<FieldDefinition<TDerived>> properties,
            string regexPattern,
            string regexOptions = "i",
            FindOptions options = null) where TDerived : TEntity
        {
            var filters = properties.Select(p =>
                Builders<TDerived>.Filter.Regex(p, new BsonRegularExpression(regexPattern, regexOptions)));
            return _mongoCollection.OfType<TDerived>()
                .Find(Builders<TDerived>.Filter.Or(filters), options);
        }

        public virtual Task<IAsyncCursor<TEntity>> FindAsync(
            Expression<Func<TEntity, bool>> filter,
            FindOptions<TEntity, TEntity> options = null,
            CancellationToken cancellationToken = default)
        {
            return _mongoCollection.FindAsync(filter, options, cancellationToken);
        }

        public virtual Task<IAsyncCursor<TEntity>> FindAsync(
            FilterDefinition<TEntity> filter,
            FindOptions<TEntity, TEntity> options = null,
            CancellationToken cancellationToken = default)
        {
            return _mongoCollection.FindAsync(filter, options, cancellationToken);
        }

        public virtual Task<IAsyncCursor<TDerived>> FindAsync<TDerived>(
            Expression<Func<TDerived, bool>> filter,
            FindOptions<TDerived, TDerived> options = null,
            CancellationToken cancellationToken = default) where TDerived : TEntity
        {
            return _mongoCollection.OfType<TDerived>().FindAsync(filter, options, cancellationToken);
        }

        public virtual Task<IAsyncCursor<TReturnProjection>> FindAsync<TReturnProjection>(
            Expression<Func<TEntity, bool>> filter,
            Expression<Func<TEntity, TReturnProjection>> returnProjection,
            FindOptions<TEntity, TReturnProjection> options = null,
            CancellationToken cancellationToken = default)
        {
            var opt = options ?? new FindOptions<TEntity, TReturnProjection>();
            opt.Projection = Builders<TEntity>.Projection.Expression(returnProjection);
            return _mongoCollection.FindAsync(filter, opt, cancellationToken);
        }

        public virtual Task<IAsyncCursor<TReturnProject>> FindAsync<TDerived, TReturnProject>(
            Expression<Func<TDerived, bool>> filter,
            Expression<Func<TDerived, TReturnProject>> returnProjection,
            FindOptions<TDerived, TReturnProject> options = null,
            CancellationToken cancellationToken = default) where TDerived : TEntity
        {
            var opt = options ?? new FindOptions<TDerived, TReturnProject>();
            opt.Projection = Builders<TDerived>.Projection.Expression(returnProjection);
            return _mongoCollection.OfType<TDerived>().FindAsync(filter, opt, cancellationToken);
        }

        public virtual async Task<TEntity> FindOneAsync(
            Expression<Func<TEntity, bool>> filter,
            CancellationToken cancellationToken = default)
        {
            return await (await _mongoCollection.FindAsync(filter, cancellationToken: cancellationToken))
                .FirstOrDefaultAsync(cancellationToken);
        }

        public virtual async Task<TDerived> FindOneAsync<TDerived>(
            Expression<Func<TDerived, bool>> filter,
            CancellationToken cancellationToken = default) where TDerived : TEntity
        {
            return await (await _mongoCollection.OfType<TDerived>()
                    .FindAsync(filter, cancellationToken: cancellationToken))
                .FirstOrDefaultAsync(cancellationToken);
        }

        public virtual async Task<TReturnProject> FindOneAsync<TReturnProject>(
            Expression<Func<TEntity, bool>> filter,
            Expression<Func<TEntity, TReturnProject>> returnProjection,
            CancellationToken cancellationToken = default)
        {
            return await (await _mongoCollection.FindAsync(
                    filter,
                    new FindOptions<TEntity, TReturnProject>
                        { Projection = Builders<TEntity>.Projection.Expression(returnProjection) },
                    cancellationToken))
                .FirstOrDefaultAsync(cancellationToken);
        }

        public virtual async Task<TReturnProject> FindOneAsync<TDerived, TReturnProject>(
            Expression<Func<TDerived, bool>> filter,
            Expression<Func<TDerived, TReturnProject>> returnProjection,
            CancellationToken cancellationToken = default) where TDerived : TEntity
        {
            return await (await _mongoCollection.OfType<TDerived>()
                    .FindAsync(
                        filter,
                        new FindOptions<TDerived, TReturnProject>
                            { Projection = Builders<TDerived>.Projection.Expression(returnProjection) },
                        cancellationToken))
                .FirstOrDefaultAsync(cancellationToken);
        }

        public virtual IFindFluent<TEntity, TEntity> FindFluent(
            FilterDefinition<TEntity> filter,
            FindOptions options = null)
        {
            return _mongoCollection.Find(filter, options);
        }

        public virtual IFindFluent<TDerived, TDerived> FindFluent<TDerived>(
            FilterDefinition<TDerived> filter,
            FindOptions options = null) where TDerived : TEntity
        {
            return _mongoCollection.OfType<TDerived>().Find(filter, options);
        }

        #endregion

        #region Find & insert

        public async Task<TProject> FindOneOrInsertAsync<TProject>(
            Expression<Func<TEntity, bool>> filter,
            TEntity entity,
            Expression<Func<TEntity, TProject>> returnProjection,
            ReturnDocument returnDocument = ReturnDocument.After, CancellationToken cancellationToken = default)
        {
            return await _mongoCollection.FindOneAndUpdateAsync<TEntity, TProject>(
                filter,
                new BsonDocumentUpdateDefinition<TEntity>(new BsonDocument("$setOnInsert",
                    entity.ToBsonDocument(_mongoCollection.DocumentSerializer))),
                new FindOneAndUpdateOptions<TEntity, TProject>
                {
                    IsUpsert = true,
                    ReturnDocument = returnDocument,
                    Projection = Builders<TEntity>.Projection.Expression(returnProjection)
                },
                cancellationToken);
        }

        #endregion

        #region Aggregate

        public virtual IAggregateFluent<TEntity> Aggregate(
            AggregateOptions options = null)
        {
            return _mongoCollection.Aggregate(options);
        }

        #endregion

        #region Update

        public virtual Task<UpdateResult> UpdateOneAsync(
            string id,
            Func<UpdateDefinitionBuilder<TEntity>, UpdateDefinition<TEntity>> update,
            bool isUpsert = false,
            CancellationToken cancellationToken = default)
        {
            return UpdateOneAsync(id, update, new UpdateOptions { IsUpsert = isUpsert }, cancellationToken);
        }

        public virtual async Task<UpdateResult> UpdateOneAsync(
            string id,
            Func<UpdateDefinitionBuilder<TEntity>, UpdateDefinition<TEntity>> update,
            UpdateOptions options,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));

            var filter = Builders<TEntity>.Filter.Eq("_id", ObjectId.Parse(id));
            return await _mongoCollection
                .UpdateOneAsync(filter, update(Builders<TEntity>.Update), options, cancellationToken)
                .ConfigureAwait(false);
        }

        public virtual Task<UpdateResult> UpdateOneAsync(
            Expression<Func<TEntity, bool>> filter,
            Func<UpdateDefinitionBuilder<TEntity>, UpdateDefinition<TEntity>> update,
            bool isUpsert = false,
            CancellationToken cancellationToken = default)
            => UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = isUpsert }, cancellationToken);

        public virtual Task<UpdateResult> UpdateOneAsync(
            FilterDefinition<TEntity> filter,
            Func<UpdateDefinitionBuilder<TEntity>, UpdateDefinition<TEntity>> update,
            bool isUpsert = false,
            CancellationToken cancellationToken = default)
            => UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = isUpsert }, cancellationToken);

        public virtual Task<UpdateResult> UpdateOneAsync(Expression<Func<TEntity, bool>> filter,
            Func<UpdateDefinitionBuilder<TEntity>, UpdateDefinition<TEntity>> update, UpdateOptions options,
            CancellationToken cancellationToken = default)
            => UpdateOneAsync((FilterDefinition<TEntity>) (filter), update, options, cancellationToken);

        public virtual async Task<UpdateResult> UpdateOneAsync(FilterDefinition<TEntity> filter,
            Func<UpdateDefinitionBuilder<TEntity>, UpdateDefinition<TEntity>> update, UpdateOptions options,
            CancellationToken cancellationToken = default)
        {
            return await _mongoCollection
                .UpdateOneAsync(filter, update(Builders<TEntity>.Update), options, cancellationToken)
                .ConfigureAwait(false);
        }

        public virtual Task<UpdateResult> UpdateOneAsync<TDerived>(
            string id,
            Func<UpdateDefinitionBuilder<TDerived>, UpdateDefinition<TDerived>> update,
            bool isUpsert = false,
            CancellationToken cancellationToken = default) where TDerived : TEntity
            => UpdateOneAsync(id, update, new UpdateOptions { IsUpsert = isUpsert }, cancellationToken);

        public virtual async Task<UpdateResult> UpdateOneAsync<TDerived>(
            string id,
            Func<UpdateDefinitionBuilder<TDerived>, UpdateDefinition<TDerived>> update,
            UpdateOptions options,
            CancellationToken cancellationToken = default) where TDerived : TEntity
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));

            var filter = Builders<TDerived>.Filter.Eq("_id", ObjectId.Parse(id));
            return await _mongoCollection.OfType<TDerived>()
                .UpdateOneAsync(filter, update(Builders<TDerived>.Update), options, cancellationToken)
                .ConfigureAwait(false);
        }

        public virtual Task<UpdateResult> UpdateOneAsync<TDerived>(
            Expression<Func<TDerived, bool>> filter,
            Func<UpdateDefinitionBuilder<TDerived>, UpdateDefinition<TDerived>> update,
            bool isUpsert = false,
            CancellationToken cancellationToken = default) where TDerived : TEntity
            => UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = isUpsert }, cancellationToken);

        public virtual Task<UpdateResult> UpdateOneAsync<TDerived>(
            FilterDefinition<TDerived> filter,
            Func<UpdateDefinitionBuilder<TDerived>, UpdateDefinition<TDerived>> update,
            bool isUpsert = false,
            CancellationToken cancellationToken = default) where TDerived : TEntity
            => UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = isUpsert }, cancellationToken);

        public virtual Task<UpdateResult> UpdateOneAsync<TDerived>(
            Expression<Func<TDerived, bool>> filter,
            Func<UpdateDefinitionBuilder<TDerived>, UpdateDefinition<TDerived>> update,
            UpdateOptions options,
            CancellationToken cancellationToken = default) where TDerived : TEntity
            => UpdateOneAsync((FilterDefinition<TDerived>) filter, update, options, cancellationToken);

        public virtual async Task<UpdateResult> UpdateOneAsync<TDerived>(
            FilterDefinition<TDerived> filter,
            Func<UpdateDefinitionBuilder<TDerived>, UpdateDefinition<TDerived>> update,
            UpdateOptions options,
            CancellationToken cancellationToken = default) where TDerived : TEntity
        {
            return await _mongoCollection.OfType<TDerived>()
                .UpdateOneAsync(filter, update(Builders<TDerived>.Update), options, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task UpdateManyAsync(
            Expression<Func<TEntity, bool>> filter,
            Func<UpdateDefinitionBuilder<TEntity>, UpdateDefinition<TEntity>> update,
            UpdateOptions options = null,
            CancellationToken cancellationToken = default)
        {
            await _mongoCollection.UpdateManyAsync(filter, update(Builders<TEntity>.Update), options, cancellationToken)
                .ConfigureAwait(false);
        }

        public virtual Task<TProjection> FindOneAndUpdateAsync<TProjection>(
            Expression<Func<TEntity, bool>> filter,
            Func<UpdateDefinitionBuilder<TEntity>, UpdateDefinition<TEntity>> update,
            Expression<Func<TEntity, TProjection>> returnProjection,
            ReturnDocument returnDocument = ReturnDocument.After,
            bool isUpsert = false,
            CancellationToken cancellationToken = default)
            => FindOneAndUpdateAsync(
                filter,
                update,
                Builders<TEntity>.Projection.Expression(returnProjection),
                returnDocument, isUpsert, cancellationToken);

        public virtual async Task<TProjection> FindOneAndUpdateAsync<TProjection>(
            Expression<Func<TEntity, bool>> filter,
            Func<UpdateDefinitionBuilder<TEntity>, UpdateDefinition<TEntity>> update,
            ProjectionDefinition<TEntity, TProjection> returnProjection,
            ReturnDocument returnDocument = ReturnDocument.After,
            bool isUpsert = false,
            CancellationToken cancellationToken = default)
        {
            return await _mongoCollection.FindOneAndUpdateAsync(
                    filter,
                    update(Builders<TEntity>.Update),
                    new FindOneAndUpdateOptions<TEntity, TProjection>
                    {
                        Projection = returnProjection,
                        ReturnDocument = returnDocument,
                        IsUpsert = isUpsert
                    },
                    cancellationToken)
                .ConfigureAwait(false);
        }

        #endregion

        #region Replace

        public async Task<ReplaceOneResult> ReplaceOneAsync(
            string id,
            TEntity entity,
            bool isUpsert = false,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));

            var filter = Builders<TEntity>.Filter.Eq("_id", ObjectId.Parse(id));
            return await _mongoCollection.ReplaceOneAsync(filter, entity, new ReplaceOptions { IsUpsert = isUpsert },
                    cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ReplaceOneResult> ReplaceOneAsync(
            Expression<Func<TEntity, bool>> filter,
            TEntity entity,
            bool isUpsert = false,
            CancellationToken cancellationToken = default)
        {
            return await _mongoCollection.ReplaceOneAsync(filter, entity, new ReplaceOptions { IsUpsert = isUpsert },
                    cancellationToken)
                .ConfigureAwait(false);
        }

        public Task<TProject> FindOneAndReplaceAsync<TProject>(
            Expression<Func<TEntity, bool>> filter,
            TEntity replacement,
            Expression<Func<TEntity, TProject>> returnProjection,
            ReturnDocument returnDocument = ReturnDocument.After,
            bool isUpsert = false,
            CancellationToken cancellationToken = default)
            => FindOneAndReplaceAsync(
                filter,
                replacement,
                Builders<TEntity>.Projection.Expression(returnProjection),
                returnDocument, isUpsert, cancellationToken);

        public async Task<TProject> FindOneAndReplaceAsync<TProject>(
            Expression<Func<TEntity, bool>> filter,
            TEntity replacement,
            ProjectionDefinition<TEntity, TProject> returnProjection,
            ReturnDocument returnDocument = ReturnDocument.After,
            bool isUpsert = false,
            CancellationToken cancellationToken = default)
        {
            return await _mongoCollection.FindOneAndReplaceAsync(
                    filter,
                    replacement,
                    new FindOneAndReplaceOptions<TEntity, TProject>
                    {
                        Projection = returnProjection,
                        ReturnDocument = returnDocument,
                        IsUpsert = isUpsert
                    },
                    cancellationToken)
                .ConfigureAwait(false);
        }

        #endregion

        #region Delete

        public virtual async Task<DeleteResult> DeleteOneAsync(string id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));
            var filter = new BsonDocumentFilterDefinition<TEntity>(new BsonDocument("_id", ObjectId.Parse(id)));

            return await DeleteOneAsync(filter, cancellationToken);
        }

        public virtual Task<DeleteResult> DeleteOneAsync(
            Expression<Func<TEntity, bool>> filter,
            CancellationToken cancellationToken = default)
            => DeleteOneAsync(Builders<TEntity>.Filter.Where(filter), cancellationToken);


        public virtual async Task<DeleteResult> DeleteOneAsync(FilterDefinition<TEntity> filter,
            CancellationToken cancellationToken = default)
        {
            return await _mongoCollection.DeleteOneAsync(filter, cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task<DeleteResult> DeleteManyAsync(
            Expression<Func<TEntity, bool>> filter,
            CancellationToken cancellationToken = default)
        {
            return await _mongoCollection.DeleteManyAsync(filter, cancellationToken);
        }

        public virtual Task<TEntity> FindOneAndDeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));

            var filter = new BsonDocumentFilterDefinition<TEntity>(new BsonDocument("_id", ObjectId.Parse(id)));
            return FindOneAndDeleteAsync(filter, cancellationToken);
        }

        public virtual async Task<TEntity> FindOneAndDeleteAsync(FilterDefinition<TEntity> filter,
            CancellationToken cancellationToken = default)
        {
            return await _mongoCollection.FindOneAndDeleteAsync(filter, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public virtual Task<TEntity> FindOneAndDeleteAsync(
            Expression<Func<TEntity, bool>> filter,
            CancellationToken cancellationToken = default)
        {
            var filterExpr = Builders<TEntity>.Filter.Where(filter);
            return FindOneAndDeleteAsync(filterExpr, cancellationToken);
        }

        #endregion
    }
}