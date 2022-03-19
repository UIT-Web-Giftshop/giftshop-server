using Application.Features.Objects.Commands.Update.UpdateOneFiledOfObject;

namespace Application.Features.Products.Commands.Update.UpdateOneFiledOfProduct.UpdatePriceOfProduct
{
    public class UpdatePriceOfProductCommand : UpdateOneFieldOfObjectCommand
    {
        public double Price { get; set; }
    }
}