using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Interfaces.Repositories;

namespace Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IMongoContext context) : base(context)
        {

        }
    }
}