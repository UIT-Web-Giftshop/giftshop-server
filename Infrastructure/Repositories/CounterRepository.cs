using System;
using System.Linq.Expressions;
using System.Reflection;
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

        public override async Task<CounterCollection> FindOneAsync(
            Expression<Func<CounterCollection, bool>> filter,
            CancellationToken cancellationToken = default)
        {
            var counter = await base.FindOneAsync(filter, cancellationToken);
            if (counter is not null)
                return counter;

            return await ResolveEmptyCollectionCounter(filter, cancellationToken);
        }

        public virtual async Task IncreaseAsync<TCollection>(int value = 1,
            CancellationToken cancellationToken = default)
            where TCollection : class
        {
            if (value == 0) return;

            Expression<Func<CounterCollection, bool>> filter = x =>
                x.CollectionName == BsonCollection.GetCollectionName<TCollection>();

            var counter = await base.FindOneAsync(filter, cancellationToken);
            if (counter != null)
            {
                await base.UpdateOneAsync(
                    filter,
                    x => x.Set(c => c.CurrentCount, counter.CurrentCount + value),
                    false,
                    cancellationToken);
            }
            else
            {
                await ResolveNotFoundCounter<TCollection>(cancellationToken);
            }
        }

        public async Task DecreaseAsync<TCollection>(int value = 1, CancellationToken cancellationToken = default)
            where TCollection : class
        {
            if (value == 0) return;

            Expression<Func<CounterCollection, bool>> filter = x =>
                x.CollectionName == BsonCollection.GetCollectionName<TCollection>();

            var counter = await base.FindOneAsync(filter, cancellationToken);
            if (counter != null)
            {
                await base.UpdateOneAsync(
                    filter,
                    x => x.Set(c => c.CurrentCount, counter.CurrentCount - value),
                    false,
                    cancellationToken);
            }
            else
            {
                await ResolveNotFoundCounter<TCollection>(cancellationToken);
            }
        }

        private async Task ResolveNotFoundCounter<TCollection>(CancellationToken cancellationToken = default)
            where TCollection : class
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

        private async Task<CounterCollection> ResolveEmptyCollectionCounter(
            Expression<Func<CounterCollection, bool>> filter, CancellationToken cancellationToken)
        {
            var exprValue = VisitRightValueOfExpr(filter);
            var exprStr = exprValue.ToString();
            
            var database = MongoContext.GetContextDatabase();
            var collection = database.GetCollection<object>(exprStr);
            var documentsCount =
                await collection.CountDocumentsAsync(Builders<object>.Filter.Empty,
                    cancellationToken: cancellationToken);
            var newCounter = new CounterCollection()
            {
                CollectionName = exprStr,
                CurrentCount = documentsCount
            };
            await base.InsertAsync(newCounter, cancellationToken);
            return newCounter;
        }

        private static object VisitRightValueOfExpr(Expression<Func<CounterCollection, bool>> filter)
        {
            var member = (MemberExpression) (((BinaryExpression) filter.Body).Right);
            var container = ((ConstantExpression) member.Expression).Value;
            var value = ((FieldInfo) member.Member).GetValue(container);
            return value;
        }
    }
}