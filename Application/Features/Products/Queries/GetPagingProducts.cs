#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Application.Features.Products.Vms;
using AutoMapper;
using Domain.Entities;
using Domain.Paging;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.Features.Products.Queries
{
    public class GetPagingProducts
    {
        public class Query : IRequest<ResponseApi<PagingModel<ProductVm>>>
        {
            public PagingRequest? PagingRequest { get; set; }
            public string? Search { get; set; }
            public string? SortBy { get; set; }
            public bool IsSortAscending { get; set; }
        }

        public class Handler : IRequestHandler<Query, ResponseApi<PagingModel<ProductVm>>>
        {
            private readonly IProductRepository _productRepository;
            private readonly IMapper _mapper;

            public Handler(IProductRepository productRepository, IMapper mapper)
            {
                _productRepository = productRepository;
                _mapper = mapper;
            }

            public async Task<ResponseApi<PagingModel<ProductVm>>> Handle(Query request, CancellationToken cancellationToken)
            {
                if (string.IsNullOrWhiteSpace(request.SortBy))
                {
                    request.SortBy = "price";
                }

                Expression<Func<Product, bool>>? expression = string.IsNullOrWhiteSpace(request.Search)
                    ? null
                    : p => p.Name.Contains(request.Search);

                var dataList = await _productRepository.GetPagingAsync(
                    request.PagingRequest, 
                    expression, 
                    request.SortBy, 
                    request.IsSortAscending, 
                    cancellationToken);
                
                var data = _mapper.Map<List<ProductVm>>(dataList);
                return ResponseApi<PagingModel<ProductVm>>.ResponseOk(new PagingModel<ProductVm>{Total = dataList.Count(), Items = data});
            }
        }
    }
}