﻿using System;
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
        private readonly ICounterRepository _counterRepository;
        public ProductRepository(IMongoContext context, ICounterRepository counterRepository) : base(context)
        {
            _counterRepository = counterRepository;
        }

        public override async Task<Product> AddAsync(Product entity, CancellationToken cancellationToken = default)
        {
            var addedProduct = await base.AddAsync(entity, cancellationToken);
            if (addedProduct is null) 
                return null;
            
            // Save total products count
            await _counterRepository.IncreaseAsync<Product>(1,cancellationToken);

            return addedProduct;
        }
        
        // !FIXME: fix
        public override async Task<bool> DeleteOneAsync(Expression<Func<Product, bool>> expression, CancellationToken cancellationToken = default)
        {
            var deletedProduct = await base.DeleteOneAsync(expression, cancellationToken);
            if (!deletedProduct)
                return false;
            
            // decrement total products count
            await _counterRepository.DecreaseAsync<Product>(-1, cancellationToken);

            return true;
        }
    }
}