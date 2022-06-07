using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using AutoMapper;
using Domain.Attributes;
using Domain.Entities;
using Domain.Paging;
using Domain.ViewModels.Product;
using Infrastructure.Interfaces.Repositories;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Application.Features.Products.Queries.GetPagingProducts
{
    public class GetPagingProductsQueryHandler : IRequestHandler<GetPagingProductsQuery, ResponseApi<PagingModel<ProductDetailViewModel>>>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICounterRepository _counterRepository;
        private readonly IMapper _mapper;

        public GetPagingProductsQueryHandler(IProductRepository productRepository, IMapper mapper, ICounterRepository counterRepository)
        {
            _productRepository = productRepository;
            _counterRepository = counterRepository;
            _mapper = mapper;
        }

        public Task<ResponseApi<PagingModel<ProductDetailViewModel>>> Handle(GetPagingProductsQuery request,
            CancellationToken cancellationToken)
        {
            var (sortDirect, sortField) = PrepareSortDefinition(request);
            var findOptions = PrepareFindOptions(request, sortField, sortDirect);
            var filter = PrepareFilter(request);

            var collectionTarget = BsonCollection.GetCollectionName<Product>();
            var totalTask = _counterRepository.FindOneAsync(x => x.CollectionName == collectionTarget, cancellationToken);
            var dataTask = _productRepository.FindAsync(filter, findOptions, cancellationToken);
            
            Task.WaitAll(new Task[] {totalTask, dataTask}, cancellationToken);

            var dataList = _mapper.Map<List<ProductDetailViewModel>>(dataTask.Result.ToList(cancellationToken));
            var viewModel = new PagingModel<ProductDetailViewModel>()
            {
                AllTotalCount = totalTask.Result.CurrentCount,
                ItemsCount = dataList.Count,
                Items = dataList
            };
            
            return Task.FromResult(ResponseApi<PagingModel<ProductDetailViewModel>>.ResponseOk(viewModel));
        }

        private static Expression<Func<Product, bool>> PrepareFilter(GetPagingProductsQuery request)
        {
            Expression<Func<Product, bool>> filter = q => q.IsActive == true;
            if (!string.IsNullOrEmpty(request.Search))
            {
                filter = q => q.IsActive == true && q.Name.Contains(request.Search);
            }

            return filter;
        }

        private static FindOptions<Product, Product> PrepareFindOptions(GetPagingProductsQuery request, string sortField, int sortDirect)
        {
            var findOptions = new FindOptions<Product, Product>
            {
                Limit = request.PageSize,
                Skip = (request.PageIndex - 1) * request.PageSize,
                Sort = new BsonDocument { { sortField, sortDirect } } //-1 giam dan, 1 tang dan
            };
            return findOptions;
        }

        private static (int, string) PrepareSortDefinition(GetPagingProductsQuery request)
        {
            var sortDirect = request.isDesc ? -1 : 1;
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