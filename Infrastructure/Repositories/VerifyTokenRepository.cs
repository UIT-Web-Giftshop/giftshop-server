using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Interfaces.Repositories;

namespace Infrastructure.Repositories
{
    public class VerifyTokenRepository : RefactorRepository<VerifyToken>, IVerifyTokenRepository
    {
        public VerifyTokenRepository(IMongoContext mongoContext) : base(mongoContext)
        {
        }

        public override async Task InsertAsync(VerifyToken entity, CancellationToken cancellationToken = default)
        {
            var token = await base.FindOneAndReplaceAsync(
                x => x.Email == entity.Email,
                entity,
                x => x.Token,
                cancellationToken: cancellationToken);
            
            if (token is null)
            {
                await base.InsertAsync(entity, cancellationToken);
            }
        }
    }
}