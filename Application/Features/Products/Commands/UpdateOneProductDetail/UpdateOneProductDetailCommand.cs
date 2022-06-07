using Application.Commons;
using Domain.ViewModels.Product;
using MediatR;

namespace Application.Features.Products.Commands
{
    public class UpdateOneProductDetailCommand : IRequest<ResponseApi<Unit>>
    {
        public ProductDetailViewModel Product { get; init; }
    }
}