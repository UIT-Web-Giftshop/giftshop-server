using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Interfaces;
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
    }
}