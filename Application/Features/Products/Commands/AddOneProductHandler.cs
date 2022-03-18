using System;
using Application.Features.Objects.Commands.AddOneObject;
using Application.Features.Products.Vms;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces.Repositories;

namespace Application.Features.Products.Commands
{
    public class AddOneProductHandler : AddOneObjectHandler<Product, ProductVm>
    {
        public AddOneProductHandler(IProductRepository _productRepository, IMapper _mapper) : 
            base(_productRepository, _mapper)
        {

        }

        public override Product AddNewValue(Product entity)
        {
            entity.CreatedAt = DateTime.Now;
            return base.AddNewValue(entity);
        }
    }
}