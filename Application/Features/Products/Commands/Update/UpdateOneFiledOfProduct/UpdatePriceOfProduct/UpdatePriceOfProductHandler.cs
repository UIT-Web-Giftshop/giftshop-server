using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.Objects.Commands.Update.UpdateOneFiledOfObject;
using Domain.Entities;
using Infrastructure.Interfaces.Repositories;

namespace Application.Features.Products.Commands.Update.UpdateOneFiledOfProduct.UpdatePriceOfProduct
{
    public class UpdatePriceOfProductHandler : UpdateOneFieldOfObjectHandler<Product, 
        UpdatePriceOfProductCommand>
    {
        public UpdatePriceOfProductHandler(IProductRepository _productRepository) : 
            base(_productRepository)
        {

        }

        public override async Task<bool> GetResult(Expression<Func<Product, bool>> expression, 
            UpdateOneFieldOfObjectCommand request, CancellationToken cancellationToken)
        {
            return await this._baseRepository.PatchOneFieldAsync(expression, p => p.Price,
                ((UpdatePriceOfProductCommand)request).Price, cancellationToken);
        }
    }
}