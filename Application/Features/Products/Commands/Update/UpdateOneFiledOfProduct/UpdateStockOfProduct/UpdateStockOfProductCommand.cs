using Application.Features.Objects.Commands.Update.UpdateOneFiledOfObject;

namespace Application.Features.Products.Commands.Update.UpdateOneFiledOfProduct.UpdateStockOfProduct
{
    public class UpdateStockOfProductCommand : UpdateOneFieldOfObjectCommand
    {
        public uint Stock { get; set; }
    }
}