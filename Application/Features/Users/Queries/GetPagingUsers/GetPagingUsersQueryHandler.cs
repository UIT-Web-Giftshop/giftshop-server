using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Domain.Attributes;
using Domain.Entities;
using Domain.Entities.Account;
using Domain.Paging;
using Infrastructure.Interfaces.Repositories;
using MediatR;
using MongoDB.Bson;
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
            
            var (sortDirect, sortField) = PrepareSortDefinition(request);
            var findOptions = PrepareFindOptions(request, sortField, sortDirect);
            var filter = PrepareFilter(request);


            var totalTask = _userRepository.CountAsync(filter, cancellationToken);
            var dataTask = _userRepository.FindAsync(filter, findOptions, cancellationToken);

            Task.WaitAll(new Task[] {totalTask, dataTask}, cancellationToken);

            var dataList = dataTask.Result.ToList(cancellationToken);
            var viewModel = new PagingModel<User>()
            {
                AllTotalCount = totalTask.Result,
                ItemsCount = dataList.Count,
                Items = dataList
            };

            return Task.FromResult(ResponseApi<PagingModel<User>>.ResponseOk(viewModel));
        }
        
        private static Expression<Func<User, bool>> PrepareFilter(GetPagingUsersQuery request)
        {
            Expression<Func<User, bool>> filter = request.SearchBy switch
            {
                "email" => x => x.Email.Contains(request.Search),
                "name" => x => x.LastName.Contains(request.Search),
                _ => x => true
            };
            return filter;
        }
        
        private static FindOptions<User, User> PrepareFindOptions(GetPagingUsersQuery request, string sortField, int sortDirect)
        {
            var findOptions = new FindOptions<User, User>
            {
                Limit = request.PageSize,
                Skip = (request.PageIndex - 1) * request.PageSize,
                Sort = new BsonDocument { { sortField, sortDirect } } //-1 giam dan, 1 tang dan
            };
            return findOptions;
        }
        
        private static (int, string) PrepareSortDefinition(GetPagingUsersQuery request)
        {
            var sortDirect = request.IsDesc ? -1 : 1;
            var sortField = request.SortBy switch
            {
                "name" => "lastname",
                "email" => "email",
                "date" => "createdAt",
                _ => "email"
            };
            return (sortDirect, sortField);
        }
    }
}