using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.Objects.Commands.Update.UpdateOneFiledOfObject;
using Domain.Entities;
using Infrastructure.Interfaces.Repositories;

namespace Application.Features.Products.Commands.Update.UpdateOneFiledOfProduct.UpdateStockOfProduct
{
    public class UpdateStockOfProductHandler : UpdateOneFieldOfObjectHandler<Product,
        UpdateStockOfProductCommand>
    {
        public UpdateStockOfProductHandler(IProductRepository _productRepository) :
            base(_productRepository)
        {

        }

        public override async Task<bool> GetResult(Expression<Func<Product, bool>> expression,
            UpdateOneFieldOfObjectCommand request, CancellationToken cancellationToken)
        {
            return await this._baseRepository.PatchOneFieldAsync(expression, p => p.Stock,
                ((UpdateStockOfProductCommand)request).Stock, cancellationToken);
        }
    }
}