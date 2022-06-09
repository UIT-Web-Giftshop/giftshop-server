using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Entities.Order;
using Infrastructure.Context;
using Infrastructure.Interfaces.Repositories;
using MongoDB.Driver;

namespace Infrastructure.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly ICounterRepository _counterRepository;

        public OrderRepository(IMongoContext context, ICounterRepository counterRepository) : base(context)
        {
            _counterRepository = counterRepository;
        }
        
        public override Task InsertAsync(Order entity, CancellationToken cancellationToken = default)
        {
            var added = base.InsertAsync(entity, cancellationToken);
            added.Wait(cancellationToken);
            Task.Run(() => _counterRepository.IncreaseAsync<Order>(1, cancellationToken), cancellationToken)
                .Wait(cancellationToken);
            return added;
        }

        public override Task InsertManyAsync(ICollection<Order> entities, CancellationToken cancellationToken = default)
        {
            var added = base.InsertManyAsync(entities, cancellationToken);
            added.Wait(cancellationToken);
            Task.Run(() => _counterRepository.IncreaseAsync<Order>(entities.Count, cancellationToken), cancellationToken)
                .Wait(cancellationToken);
            return added;
        }

        public override Task InsertManyAsync<TDerived>(
            ICollection<TDerived> entities, 
            CancellationToken cancellationToken = default)
        {
            var added = base.InsertManyAsync(entities, cancellationToken);
            added.Wait(cancellationToken);
            Task.Run(() => _counterRepository.IncreaseAsync<Order>(entities.Count, cancellationToken), cancellationToken)
                .Wait(cancellationToken);
            return added;
        }

        public override async Task<DeleteResult> DeleteManyAsync(Expression<Func<Order, bool>> filter, CancellationToken cancellationToken = default)
        {
            try
            {
                var deleted = await base.DeleteManyAsync(filter, cancellationToken);
                await _counterRepository.DecreaseAsync<Order>((int)deleted.DeletedCount, cancellationToken);
                return deleted;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        
        public override async Task<DeleteResult> DeleteOneAsync(Expression<Func<Order, bool>> filter, CancellationToken cancellationToken = default)
        {
            var deleted = await base.DeleteOneAsync(filter, cancellationToken);
            await _counterRepository.DecreaseAsync<Order>((int)deleted.DeletedCount, cancellationToken);
            return deleted;
        }
    }
}