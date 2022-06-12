using System;
using System.Collections.Generic; 
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using AutoMapper;
using Domain.Attributes;
using Domain.Entities;
using Domain.Paging;
using Infrastructure.Interfaces.Repositories;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Application.Features.Products.Queries.GetPagingProducts
{
    public class GetPagingProductsManagedQueryHandler : IRequestHandler<GetPagingProductsManagedQuery, ResponseApi<PagingModel<Product>>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ICounterRepository _counterRepository;

        public GetPagingProductsManagedQueryHandler(IProductRepository productRepository, IMapper mapper, ICounterRepository counterRepository)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _counterRepository = counterRepository;
        }

        public Task<ResponseApi<PagingModel<Product>>> Handle(GetPagingProductsManagedQuery request, CancellationToken cancellationToken)
        {
            var (sortDirect, sortField) = PrepareSortDefinition(request);
            var findOptions = PrepareFindOptions(request, sortField, sortDirect);
            var filter = PrepareFilter(request);
            
            // var collectionTarget = BsonCollection.GetCollectionName<Product>();
            var totalTask = _productRepository.CountAsync(filter, cancellationToken);
            var dataTask = _productRepository.FindAsync(filter, findOptions, cancellationToken);
            
            Task.WaitAll(new Task[] {totalTask, dataTask}, cancellationToken);

            var dataList = _mapper.Map<List<Product>>(dataTask.Result.ToList(cancellationToken));
            var viewModel = new PagingModel<Product>()
            {
                AllTotalCount = totalTask.Result,
                ItemsCount = dataList.Count,
                Items = dataList
            };
            
            return Task.FromResult(ResponseApi<PagingModel<Product>>.ResponseOk(viewModel));
        }
        
        private static FilterDefinition<Product> PrepareFilter(GetPagingProductsManagedQuery request)
        {
            var filter = Builders<Product>.Filter.Empty;
            if (!string.IsNullOrEmpty(request.Search))
            {
                var lowerSearch = request.Search.ToLower();
                filter = filter & Builders<Product>.Filter.Where(q => q.Name.ToLower().Contains(lowerSearch));
            }

            if (request.ActiveStatus != "all")
            {
                var status = request.ActiveStatus == "true" ? true : false;
                filter = filter & Builders<Product>.Filter.Eq("isActive", status);
            }

            return filter;
        }

        private static FindOptions<Product, Product> PrepareFindOptions(GetPagingProductsManagedQuery request, string sortField, int sortDirect)
        {
            var findOptions = new FindOptions<Product, Product>
            {
                Limit = request.PageSize,
                Skip = (request.PageIndex - 1) * request.PageSize,
                Sort = new BsonDocument { { sortField, sortDirect } } //-1 giam dan, 1 tang dan
            };
            return findOptions;
        }

        private static (int, string) PrepareSortDefinition(GetPagingProductsManagedQuery request)
        {
            var sortDirect = request.IsDesc ? -1 : 1;
            string sortField = request.SortBy switch
            {
                "name" => "name",
                "price" => "price",
                _ => "price"
            };
            return (sortDirect, sortField);
        }
    }
}