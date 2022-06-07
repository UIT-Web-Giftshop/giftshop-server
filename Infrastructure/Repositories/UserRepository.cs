using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Interfaces.Repositories;

namespace Infrastructure.Repositories
{
    public class UserRepository : RefactorRepository<User>, IUserRepository
    {
        public UserRepository(IMongoContext context) : base(context)
        {

        }
    }
}