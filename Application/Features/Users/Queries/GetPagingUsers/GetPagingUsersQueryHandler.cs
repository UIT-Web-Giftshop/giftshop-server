using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Domain.Attributes;
using Domain.Entities;
using Domain.Paging;
using Infrastructure.Interfaces.Repositories;
using MediatR;
using MongoDB.Driver;

namespace Application.Features.Users.Queries.GetPagingUsers
{
    public class GetPagingUsersQueryHandler : IRequestHandler<GetPagingUsersQuery, ResponseApi<PagingModel<User>>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICounterRepository _counterRepository;

        public GetPagingUsersQueryHandler(IUserRepository userRepository, ICounterRepository counterRepository)
        {
            _userRepository = userRepository;
            _counterRepository = counterRepository;
        }

        public Task<ResponseApi<PagingModel<User>>> Handle(GetPagingUsersQuery request, CancellationToken cancellationToken)
        {
            var findOptions = new FindOptions<User, User>()
            {
                Limit = request.PageSize,
                Skip = (request.PageIndex - 1) * request.PageSize
            };

            var collectionTarget = BsonCollection.GetCollectionName<User>();
            var totalTask = _counterRepository.FindOneAsync(x => x.CollectionName == collectionTarget, cancellationToken);
            var dataTask = _userRepository.FindAsync(x => true, findOptions, cancellationToken);

            Task.WaitAll(new Task[] {totalTask, dataTask}, cancellationToken);

            var dataList = dataTask.Result.ToList(cancellationToken);
            var viewModel = new PagingModel<User>()
            {
                AllTotalCount = totalTask.Result.CurrentCount,
                ItemsCount = dataList.Count,
                Items = dataList
            };

            return Task.FromResult(ResponseApi<PagingModel<User>>.ResponseOk(viewModel));
        }
    }
}