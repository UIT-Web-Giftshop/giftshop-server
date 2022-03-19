using Application.Features.Objects.Commands.Update.UpdateOneObject;
using Application.Features.Products.Vms;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces.Repositories;

namespace Application.Features.Products.Commands.Update.UpdateOneProductInfo
{
    public class UpdateOneProductInfoHandler : UpdateOneObjectInfoHandler<Product, ProductVm>
    {
        public UpdateOneProductInfoHandler(IProductRepository _productRepository, IMapper _mapper) : 
            base(_productRepository, _mapper)
        {

        }
    }
}