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
    }
}