using Application.Features.Objects.Commands.Delete.DeleteOneObject;
using Domain.Entities;
using Infrastructure.Interfaces.Repositories;

namespace Application.Features.Products.Commands.Delete.DeleteOneProduct
{
    public class DeleteOneProductHandler : DeleteOneObjectHandler<Product>
    {
        public DeleteOneProductHandler(IProductRepository _productRepository) : base(_productRepository)
        {
            
        }
    }
}