using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Domain.Attributes;
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

            #region Increase product counts

            Expression<Func<SaveFlag, bool>> expression = x => x.CollectionName == BsonCollection.GetCollectionName<Product>();
            var productCounts = await _saveFlagRepository.GetOneAsync(expression, cancellationToken);
            if (productCounts != null)
            {
                await _saveFlagRepository.PatchOneFieldAsync(
                    expression,
                    p => p.CurrentCount,
                    productCounts.CurrentCount + 1,
                    cancellationToken);
            }
            else
            {
                // auto create document if not exist
                var productFlag = new SaveFlag()
                {
                    CollectionName = BsonCollection.GetCollectionName<Product>(),
                    CurrentCount = 1
                };
                await _saveFlagRepository.AddAsync(productFlag, cancellationToken);
            }
            
            #endregion

            return addedProduct;
        }
    }
}