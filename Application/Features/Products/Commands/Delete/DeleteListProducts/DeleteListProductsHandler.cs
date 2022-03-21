using Application.Features.Objects.Commands.Delete.DeleteListObjects;
using Domain.Entities;
using Infrastructure.Interfaces.Repositories;

namespace Application.Features.Products.Commands.Delete.DeleteListProducts
{
    public class DeleteListProductsHandler : DeleteListObjectsHandler<Product>
    {
        public DeleteListProductsHandler(IProductRepository _productRepository) : base(_productRepository)
        {

        }
    }
}