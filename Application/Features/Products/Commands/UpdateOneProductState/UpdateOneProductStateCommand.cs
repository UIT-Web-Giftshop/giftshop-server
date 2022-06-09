using Application.Commons;
using MediatR;

namespace Application.Features.Products.Commands.UpdateOneProductState
{
    public enum ProductState
    {
        Active = 1,
        Inactive = 0
    }
    
    public class UpdateOneProductStateCommand : IRequest<ResponseApi<Unit>>
    {
        public string Sku { get; set; }
        public ProductState State { get; set; }
    }
}