using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Interfaces;

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
            
            Expression<Func<SaveFlag, bool>> expression = x => x.CollectionName == "products";
            var productCounts = await _saveFlagRepository.GetOneAsync(expression, cancellationToken);
            await _saveFlagRepository.PatchOneFieldAsync(
                expression, 
                p => p.CurrentCount,
                productCounts.CurrentCount + 1,
                cancellationToken);

            return addedProduct;
        }
    }
}