using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Domain.Attributes;
using Domain.Entities.Order;
using Domain.Paging;
using Infrastructure.Interfaces.Repositories;
using Infrastructure.Interfaces.Services;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Application.Features.Orders.Queries.GetPagingOrders
{
    public class GetPagingProfileOrdersQueryHandler : IRequestHandler<GetPagingProfileOrdersQuery,
            ResponseApi<PagingModel<Order>>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IAccessorService _accessorService;
        private readonly ICounterRepository _counterRepository;

        public GetPagingProfileOrdersQueryHandler(IOrderRepository orderRepository, IAccessorService accessorService, ICounterRepository counterRepository)
        {
            _orderRepository = orderRepository;
            _accessorService = accessorService;
            _counterRepository = counterRepository;
        }

        public Task<ResponseApi<PagingModel<Order>>> Handle(GetPagingProfileOrdersQuery request,
            CancellationToken cancellationToken)
        {
            var email = _accessorService.Email();
            var (sortDirect, sortField) = PrepareSortDefinition(request);
            var findOptions = PrepareFindOptions(request, sortField, sortDirect);
            Expression<Func<Order, bool>> filter = q => q.UserEmail == email;
            
            var collectionTarget = BsonCollection.GetCollectionName<Order>();
            var totalTask = _counterRepository.FindOneAsync(x => x.CollectionName == collectionTarget, cancellationToken);
            var dataTask = _orderRepository.FindAsync(filter, findOptions, cancellationToken);
            
            Task.WaitAll(new Task[] {totalTask, dataTask}, cancellationToken);
            var dataList = dataTask.Result.ToList();
            
            var viewModel = new PagingModel<Order>()
            {
                AllTotalCount = totalTask.Result.CurrentCount,
                ItemsCount = dataList.Count,
                Items = dataList
            };
            
            return Task.FromResult(ResponseApi<PagingModel<Order>>.ResponseOk(viewModel));
        }

        private static FindOptions<Order, Order> PrepareFindOptions(GetPagingProfileOrdersQuery request, string sortField, int sortDirect)
        {
            var findOptions = new FindOptions<Order, Order>
            {
                Limit = request.PagingRequest.PageSize,
                Skip = (request.PagingRequest.PageIndex - 1) * request.PagingRequest.PageSize,
                Sort = new BsonDocument { { sortField, sortDirect } } //-1 giam dan, 1 tang dan
            };
            return findOptions;
        }

        private static (int, string) PrepareSortDefinition(GetPagingProfileOrdersQuery request)
        {
            var sortDirect = request.IsDesc ? -1 : 1;
            string sortField = request.SortBy switch
            {
                _ => "createdAt"
            };
            return (sortDirect, sortField);
        }
    }
}