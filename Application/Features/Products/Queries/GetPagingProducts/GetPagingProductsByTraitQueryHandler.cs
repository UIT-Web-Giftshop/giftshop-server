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
    public class GetPagingProductByTraitQueryHandler : IRequestHandler<GetPagingProductByTraitQuery,
        ResponseApi<PagingModel<ProductDetailViewModel>>>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICounterRepository _counterRepository;
        private readonly IMapper _mapper;

        public GetPagingProductByTraitQueryHandler(IProductRepository productRepository, IMapper mapper,
            ICounterRepository counterRepository)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _counterRepository = counterRepository;
        }

        public Task<ResponseApi<PagingModel<ProductDetailViewModel>>> Handle(GetPagingProductByTraitQuery request,
            CancellationToken cancellationToken)
        {
            var (sortDirect, sortField) = PrepareSortDefinition(request);
            var findOptions = PrepareFindOptions(request, sortField, sortDirect);
            var filter = PrepareFilter(request);


            var totalTask = _productRepository.CountAsync(filter, cancellationToken);
                var dataTask = _productRepository.FindAsync(filter, findOptions, cancellationToken);

            Task.WaitAll(new Task[] { totalTask, dataTask }, cancellationToken);

            var dataSrc = dataTask.Result.ToListAsync(cancellationToken).Result;
            var dataList = _mapper.Map<List<ProductDetailViewModel>>(dataSrc);
            var viewModel = new PagingModel<ProductDetailViewModel>
            {
                AllTotalCount = totalTask.Result,
                ItemsCount = dataList.Count,
                Items = dataList
            };

            return Task.FromResult(ResponseApi<PagingModel<ProductDetailViewModel>>.ResponseOk(viewModel));
        }

        private static FilterDefinition<Product> PrepareFilter(GetPagingProductByTraitQuery request)
        {
            var filter = Builders<Product>.Filter.Empty;
            if (!string.IsNullOrEmpty(request.Trait))
            {
                filter = Builders<Product>.Filter.Where(x => x.Traits.Contains(request.Trait));
            }

            return filter;
        }

        private static FindOptions<Product, Product> PrepareFindOptions(GetPagingProductByTraitQuery request,
            string sortField, int sortDirect)
        {
            var findOptions = new FindOptions<Product, Product>
            {
                Limit = request.PagingRequest.PageSize,
                Skip = (request.PagingRequest.PageIndex - 1) * request.PagingRequest.PageSize,
                Sort = new BsonDocument { { sortField, sortDirect } } //-1 giam dan, 1 tang dan
            };
            return findOptions;
        }

        private static (int, string) PrepareSortDefinition(GetPagingProductByTraitQuery request)
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