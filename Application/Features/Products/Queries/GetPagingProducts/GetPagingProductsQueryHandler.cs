using System.Collections.Generic;
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

        public async Task<ResponseApi<PagingModel<ProductDetailViewModel>>> Handle(GetPagingProductsQuery request,
            CancellationToken cancellationToken)
        {
            var findOptions = new FindOptions<Product, Product>()
            {
                Limit = request.PageSize,
                Skip = (request.PageIndex - 1) * request.PageSize
            };
            
            var collectionTarget = BsonCollection.GetCollectionName<Product>();
            var totalTask = _counterRepository.FindOneAsync(x => x.CollectionName == collectionTarget, cancellationToken);
            var dataTask = _productRepository.FindAsync(x => true, findOptions, cancellationToken);
            
            Task.WaitAll(new Task[] {totalTask, dataTask}, cancellationToken);

            var dataList = _mapper.Map<List<ProductDetailViewModel>>(dataTask.Result.ToList(cancellationToken));
            var viewModel = new PagingModel<ProductDetailViewModel>()
            {
                AllTotalCount = totalTask.Result.CurrentCount,
                ItemsCount = dataList.Count,
                Items = dataList
            };
            
            return ResponseApi<PagingModel<ProductDetailViewModel>>.ResponseOk(viewModel);
        }
    }
}