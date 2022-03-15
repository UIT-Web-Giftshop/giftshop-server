using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Domain.Attributes;
using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Interfaces.Repositories;
using MongoDB.Driver;

namespace Infrastructure.Repositories
{
    public class SaveFlagRepository : BaseRepository<SaveFlag>, ISaveFlagRepository
    {
        public SaveFlagRepository(IMongoContext context) : base(context)
        {
        }

        public override async Task<SaveFlag> GetOneAsync(Expression<Func<SaveFlag, bool>> expression, CancellationToken cancellationToken = default)
        {
            var count = await base.GetOneAsync(expression, cancellationToken);
            if (count != null) return count;

            var op = expression.Body as BinaryExpression;
            var exprValue = ((ConstantExpression) op!.Right).Value!.ToString();
            
            var database = _context.GetContextDatabase();
            var collection = database.GetCollection<object>(exprValue);
            var docsCount = await collection
                .CountDocumentsAsync(Builders<object>.Filter.Empty, cancellationToken: cancellationToken);
            var newFlag = new SaveFlag()
            {
                CollectionName = exprValue,
                CurrentCount = docsCount
            };
            await base.AddAsync(newFlag, cancellationToken);
            return newFlag;
        }

        public virtual async Task AutoIncrementFlag<TCollection>(CancellationToken cancellationToken = default) 
            where TCollection : class
        {
            Expression<Func<SaveFlag, bool>> expression = x =>
                x.CollectionName == BsonCollection.GetCollectionName<TCollection>();

            var flagCount = await base.GetOneAsync(expression, cancellationToken);
            if (flagCount != null)
            {
                await base.PatchOneFieldAsync(
                    expression,
                    x => x.CurrentCount,
                    flagCount.CurrentCount + 1,
                    cancellationToken);
            }
            else
            {
                var targetCollection = _context.GetCollection<TCollection>();
                var documentsCount = await targetCollection
                    .CountDocumentsAsync(Builders<TCollection>.Filter.Empty, cancellationToken: cancellationToken);

                var newFlag = new SaveFlag()
                {
                    CollectionName = BsonCollection.GetCollectionName<TCollection>(),
                    CurrentCount = documentsCount
                };
                await base.AddAsync(newFlag, cancellationToken);
            }
        }

        public async Task AutoDecrementFlag<TCollection>(CancellationToken cancellationToken = default) 
            where TCollection : class
        {
            Expression<Func<SaveFlag, bool>> expression = x =>
                x.CollectionName == BsonCollection.GetCollectionName<TCollection>();

            var flagCount = await base.GetOneAsync(expression, cancellationToken);
            if (flagCount != null)
            {
                await base.PatchOneFieldAsync(
                    expression,
                    x => x.CurrentCount,
                    flagCount.CurrentCount - 1,
                    cancellationToken);
            }
            else
            {
                var targetCollection = _context.GetCollection<TCollection>();
                var documentsCount = await targetCollection
                    .CountDocumentsAsync(Builders<TCollection>.Filter.Empty, cancellationToken: cancellationToken);

                var newFlag = new SaveFlag()
                {
                    CollectionName = BsonCollection.GetCollectionName<TCollection>(),
                    CurrentCount = documentsCount
                };
                await base.AddAsync(newFlag, cancellationToken);
            }
        }
    }
}