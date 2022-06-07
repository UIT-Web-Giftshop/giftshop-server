﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Application.Features.Products.Vms;
using AutoMapper;
using Domain.Attributes;
using Domain.Entities;
using Domain.Paging;
using FluentValidation;
using Infrastructure.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Products.Queries
{
    public class GetPagingProductsQuery : IRequest<ResponseApi<PagingModel<ProductVm>>>
    {
        [DefaultValue(1)]
        public int PageIndex { get; set; }
        [DefaultValue(20)]
        public int PageSize { get; set; }
        public string Search { get; set; }
        [DefaultValue("price")]
        public string SortBy { get; set; }
        [DefaultValue(false)]
        public bool IsSortAscending { get; set; }
    }

    public sealed class GetPagingProductQueryValidator : AbstractValidator<GetPagingProductsQuery>
    {
        public GetPagingProductQueryValidator()
        {
            RuleFor(x => x.PageIndex)
                .GreaterThan(0)
                .LessThan(10000);
            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .LessThan(100);
        }
    }

    public class GetPagingProductsHandler : IRequestHandler<GetPagingProductsQuery, ResponseApi<PagingModel<ProductVm>>>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICounterRepository _counterRepository;
        private readonly IMapper _mapper;

        public GetPagingProductsHandler(
            IProductRepository productRepository, 
            ICounterRepository counterRepository,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _counterRepository = counterRepository;
            _mapper = mapper;
        }

        public async Task<ResponseApi<PagingModel<ProductVm>>> Handle(GetPagingProductsQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Product, bool>> expression = string.IsNullOrWhiteSpace(request.Search)
                ? null
                : p => p.Name.Contains(request.Search);
            
            //todo: default sort by price, extract more sort option later
            Expression<Func<Product, object>> sortExpr = q => q.Price;

            var dataList = await _productRepository.GetPagingAsync(
                new PagingRequest(){PageIndex = request.PageIndex, PageSize = request.PageSize},
                expression, 
                sortExpr,
                request.IsSortAscending,
                cancellationToken);

            // check if there's not anything in collection
            if (dataList.Count() == 0)
            {
                return ResponseApi<PagingModel<ProductVm>>.ResponseOk(new PagingModel<ProductVm>()
                {
                    AllTotalCount = 0,
                    ItemsCount = 0,
                    Items = null
                });
            }

            // Attempt to get total count of products
            CounterCollection counterCollection;
            try
            {
                counterCollection = await _counterRepository
                    .FindOneAsync(
                        x => x.CollectionName == BsonCollection.GetCollectionName<Product>(), 
                        cancellationToken);
            }
            catch (Exception)
            {
                await _counterRepository.InsertAsync(new CounterCollection()
                {
                    CollectionName = BsonCollection.GetCollectionName<Product>(),
                    CurrentCount = dataList.Count()
                }, cancellationToken);
                counterCollection = await _counterRepository
                    .FindOneAsync(
                        x => x.CollectionName == BsonCollection.GetCollectionName<Product>(), 
                        cancellationToken);
            }

            var data = _mapper.Map<List<ProductVm>>(dataList);

            return ResponseApi<PagingModel<ProductVm>>.ResponseOk(new PagingModel<ProductVm>
            {
                AllTotalCount = counterCollection.CurrentCount, 
                ItemsCount = dataList.Count(), 
                Items = data
            });
        }
    }
}