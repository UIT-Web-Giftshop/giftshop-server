using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Interfaces.Repositories;

namespace Infrastructure.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        private readonly ISaveFlagRepository _saveFlagRepository;
        public ProductRepository(IMongoContext context, ISaveFlagRepository saveFlagRepository) : base(context)
        {
            _saveFlagRepository = saveFlagRepository;
        }

        public override async Task<Product> AddAsync(Product entity, CancellationToken cancellationToken = default)
        {
            var addedProduct = await base.AddAsync(entity, cancellationToken);
            if (addedProduct == null) 
                return null;
            
            // Save total products count
            await _saveFlagRepository.AutoIncrementFlag<Product>(cancellationToken);

            return addedProduct;
        }

        public override async Task<bool> DeleteOneAsync(Expression<Func<Product, bool>> expression, CancellationToken cancellationToken = default)
        {
            var deletedProduct = await base.DeleteOneAsync(expression, cancellationToken);
            if (!deletedProduct)
                return false;
            
            // decrement total products count
            await _saveFlagRepository.AutoDecrementFlag<Product>(cancellationToken);

            return true;
        }
    }
}