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
    public class CounterRepository : RefactorRepository<CounterCollection>, ICounterRepository
    {
        public CounterRepository(IMongoContext context) : base(context)
        {
        }

        public override async Task<CounterCollection> FindOneAsync(Expression<Func<CounterCollection, bool>> filter, CancellationToken cancellationToken = default)
        {
            var counter = await base.FindOneAsync(filter, cancellationToken);
            if (counter is not null)
                return counter;

            return await ResolveEmptyCollectionCounter(filter, cancellationToken);
        }

        public virtual async Task IncreaseCounter<TCollection>(CancellationToken cancellationToken = default) 
            where TCollection : class
        {
            Expression<Func<CounterCollection, bool>> filter = x =>
                x.CollectionName == BsonCollection.GetCollectionName<TCollection>();

            var counter = await base.FindOneAsync(filter, cancellationToken);
            if (counter != null)
            {
                await base.UpdateOneAsync(
                    filter,
                    x => x.Set(c => c.CurrentCount, counter.CurrentCount + 1),
                    false,
                    cancellationToken);
            }
            else
            {
                await ResolveNotFoundCounter<TCollection>(cancellationToken);
            }
        }

        public async Task DecreaseCounter<TCollection>(CancellationToken cancellationToken = default) 
            where TCollection : class
        {
            Expression<Func<CounterCollection, bool>> filter = x =>
                x.CollectionName == BsonCollection.GetCollectionName<TCollection>();

            var counter = await base.FindOneAsync(filter, cancellationToken);
            if (counter != null)
            {
                await base.UpdateOneAsync(
                    filter,
                    x => x.Set(c => c.CurrentCount, counter.CurrentCount - 1),
                    false,
                    cancellationToken);
            }
            else
            {
                await ResolveNotFoundCounter<TCollection>(cancellationToken);
            }
        }

        private async Task ResolveNotFoundCounter<TCollection>(CancellationToken cancellationToken = default) where TCollection : class
        {
            var targetCollection = MongoContext.GetCollection<TCollection>();
            var documentsCount = await targetCollection
                .CountDocumentsAsync(Builders<TCollection>.Filter.Empty, cancellationToken: cancellationToken);

            var newCounter = new CounterCollection()
            {
                CollectionName = BsonCollection.GetCollectionName<TCollection>(),
                CurrentCount = documentsCount
            };
            await base.InsertAsync(newCounter, cancellationToken);
        }

        private async Task<CounterCollection> ResolveEmptyCollectionCounter(Expression<Func<CounterCollection, bool>> filter, CancellationToken cancellationToken)
        {
            var op = filter.Body as BinaryExpression;
            var exprValue = ((ConstantExpression) op!.Right).Value!.ToString();

            var database = MongoContext.GetContextDatabase();
            var collection = database.GetCollection<object>(exprValue);
            var documentsCount =
                await collection.CountDocumentsAsync(Builders<object>.Filter.Empty,
                    cancellationToken: cancellationToken);
            var newCounter = new CounterCollection()
            {
                CollectionName = exprValue,
                CurrentCount = documentsCount
            };
            await base.InsertAsync(newCounter, cancellationToken);
            return newCounter;
        }
    }
}