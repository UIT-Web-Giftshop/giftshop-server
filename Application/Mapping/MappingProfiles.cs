using Application.Features.Orders.Vms;
using Application.Features.Products.Vms;
using Application.Features.Users.Vms;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Order;

namespace Application.Mapping
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductVm>().ReverseMap();
            CreateMap<User, UserVm>().ReverseMap();
            CreateMap<Order, OrderVm>().ReverseMap();
        }
    }
}