using System.ComponentModel;
using Application.Commons;
using Domain.Paging;
using Domain.ViewModels.Product;
using FluentValidation;
using MediatR;

namespace Application.Features.Products.Queries.GetPagingProducts
{
    public class GetPagingProductsQuery : IRequest<ResponseApi<PagingModel<ProductDetailViewModel>>>
    {
        [DefaultValue(1)] public int PageIndex { get; set; } = 1;

        [DefaultValue(20)] public int PageSize { get; set; } = 20;
        
        public string Search { get; set; }
        
        public string Trait { get; set; }
        
        public string SortBy { get; set; }
        
        [DefaultValue(true)] public bool IsDesc { get; set; }
    }

    public class GetPagingProductsQueryValidator : AbstractValidator<GetPagingProductsQuery>
    {
        public GetPagingProductsQueryValidator()
        {
            RuleFor(x => x.PageIndex).GreaterThan(0).LessThan(10000);
            RuleFor(x => x.PageSize).GreaterThan(0).LessThan(100);
        }
    }
}