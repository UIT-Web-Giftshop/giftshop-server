using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Interfaces.Repositories;
using MongoDB.Driver;

namespace Infrastructure.Repositories
{
    public class UserRepository : RefactorRepository<User>, IUserRepository
    {
        private readonly ICounterRepository _counterRepository;

        public UserRepository(IMongoContext mongoContext, ICounterRepository counterRepository) : base(mongoContext)
        {
            _counterRepository = counterRepository;
        }

        public override Task InsertAsync(User entity, CancellationToken cancellationToken = default)
        {
            var added = base.InsertAsync(entity, cancellationToken);
            added.Wait(cancellationToken);
            Task.Run(() => _counterRepository.IncreaseAsync<User>(1, cancellationToken), cancellationToken)
                .Wait(cancellationToken);
            return added;
        }

        public override Task InsertManyAsync<TDerived>(
            ICollection<TDerived> entities, 
            CancellationToken cancellationToken = default)
        {
            var added = base.InsertManyAsync(entities, cancellationToken);
            added.Wait(cancellationToken);
            Task.Run(() => _counterRepository.IncreaseAsync<User>(entities.Count, cancellationToken), cancellationToken)
                .Wait(cancellationToken);
            return added;
        }

        public override async Task<DeleteResult> DeleteManyAsync(Expression<Func<User, bool>> filter, CancellationToken cancellationToken = default)
        {
            try
            {
                var deleted = await base.DeleteManyAsync(filter, cancellationToken);
                await _counterRepository.DecreaseAsync<User>((int)deleted.DeletedCount, cancellationToken);
                return deleted;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            
        }
        
        public override async Task<DeleteResult> DeleteOneAsync(Expression<Func<User, bool>> filter, CancellationToken cancellationToken = default)
        {
            try
            {
                var deleted = await base.DeleteOneAsync(filter, cancellationToken);
                await _counterRepository.DecreaseAsync<User>((int)deleted.DeletedCount, cancellationToken);
                return deleted;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}