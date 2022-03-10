using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories
{
    public class SaveFlagRepository : BaseRepository<SaveFlag>, ISaveFlagRepository
    {
        public SaveFlagRepository(IMongoContext context) : base(context)
        {
        }
    }
}