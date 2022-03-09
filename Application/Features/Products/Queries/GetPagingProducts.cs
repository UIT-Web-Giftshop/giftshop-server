using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Application.Features.Products.Vms;
using AutoMapper;
using Domain.Entities;
using Domain.Paging;
using FluentValidation;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.Features.Products.Queries
{
    public class GetPagingProductsQuery : IRequest<ResponseApi<PagingModel<ProductVm>>>
    {
        public PagingRequest PagingRequest { get; init; }
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
            RuleFor(x => x.PagingRequest!.PageIndex)
                .GreaterThan(0)
                .LessThan(10000);
            RuleFor(x => x.PagingRequest!.PageSize)
                .GreaterThan(0)
                .LessThan(100);
        }
    }

    public class GetPagingProductsHandler : IRequestHandler<GetPagingProductsQuery, ResponseApi<PagingModel<ProductVm>>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetPagingProductsHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ResponseApi<PagingModel<ProductVm>>> Handle(GetPagingProductsQuery request, CancellationToken cancellationToken)
        {
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