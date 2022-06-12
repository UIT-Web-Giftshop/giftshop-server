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
    public class GetPagingManagedOrdersQueryHandler : IRequestHandler<GetPagingManagedOrdersQuery, ResponseApi<PagingModel<Order>>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IAccessorService _accessorService;
        private readonly ICounterRepository _counterRepository;

        public GetPagingManagedOrdersQueryHandler(IOrderRepository orderRepository, IAccessorService accessorService, ICounterRepository counterRepository)
        {
            _orderRepository = orderRepository;
            _accessorService = accessorService;
            _counterRepository = counterRepository;
        }

        public Task<ResponseApi<PagingModel<Order>>> Handle(GetPagingManagedOrdersQuery request, CancellationToken cancellationToken)
        {
            var (sortDirect, sortField) = PrepareSortDefinition(request);
            var findOptions = PrepareFindOptions(request, sortField, sortDirect);
            var filter = PrepareFilter(request);


            var totalTask = _orderRepository.CountAsync(filter, cancellationToken);
            var dataTask = _orderRepository.FindAsync(filter, findOptions, cancellationToken);
            
            Task.WaitAll(new Task[] {totalTask, dataTask}, cancellationToken);
            var dataList = dataTask.Result.ToList();
            
            var viewModel = new PagingModel<Order>()
            {
                AllTotalCount = totalTask.Result,
                ItemsCount = dataList.Count,
                Items = dataList
            };
            
            return Task.FromResult(ResponseApi<PagingModel<Order>>.ResponseOk(viewModel));
        }

        private static FilterDefinition<Order> PrepareFilter(GetPagingManagedOrdersQuery request)
        {
            var filter = Builders<Order>.Filter.Empty;
            if (!string.IsNullOrEmpty(request.Status))
            {
                filter = filter & Builders<Order>.Filter.Eq(x => x.Status, request.Status);
            }
            if (!string.IsNullOrEmpty(request.FilterUser))
            {
                filter = filter & Builders<Order>.Filter.Regex(x => x.UserEmail, request.FilterUser);
            }

            return filter;
        }

        private static FindOptions<Order, Order> PrepareFindOptions(GetPagingManagedOrdersQuery request, string sortField, int sortDirect)
        {
            var findOptions = new FindOptions<Order, Order>
            {
                Limit = request.PagingRequest.PageSize,
                Skip = (request.PagingRequest.PageIndex - 1) * request.PagingRequest.PageSize,
                Sort = new BsonDocument { { sortField, sortDirect } } //-1 giam dan, 1 tang dan
            };
            return findOptions;
        }

        private static (int, string) PrepareSortDefinition(GetPagingManagedOrdersQuery request)
        {
            var sortDirect = request.IsDesc ? -1 : 1;
            string sortField = request.SortBy switch
            {
                "price" => "totalPrice",
                _ => "createdAt"
            };
            return (sortDirect, sortField);
        }
    }
}