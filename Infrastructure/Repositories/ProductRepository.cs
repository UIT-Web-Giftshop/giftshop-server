using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Interfaces.Repositories;
using MongoDB.Driver;

namespace Infrastructure.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ICounterRepository _counterRepository;
        public ProductRepository(IMongoContext context, ICounterRepository counterRepository) : base(context)
        {
            _counterRepository = counterRepository;
        }

        public override Task InsertAsync(Product entity, CancellationToken cancellationToken = default)
        {
            var added = base.InsertAsync(entity, cancellationToken);
            added.Wait(cancellationToken);
            Task.Run(() => _counterRepository.IncreaseAsync<Product>(1, cancellationToken), cancellationToken)
                .Wait(cancellationToken);
            return added;
        }
        
        public override async Task<DeleteResult> DeleteOneAsync(Expression<Func<Product, bool>> filter, CancellationToken cancellationToken = default)
        {
            var deleted = await base.DeleteOneAsync(filter, cancellationToken);
            await _counterRepository.DecreaseAsync<Product>((int)deleted.DeletedCount, cancellationToken);
            return deleted;
        }
        
        public override async Task<DeleteResult> DeleteManyAsync(Expression<Func<Product, bool>> filter, CancellationToken cancellationToken = default)
        {
            try
            {
                var deleted = await base.DeleteManyAsync(filter, cancellationToken);
                await _counterRepository.DecreaseAsync<Product>((int)deleted.DeletedCount, cancellationToken);
                return deleted;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            
        }
    }
}