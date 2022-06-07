using Application.Features.Auths.SignupUser;
using Application.Features.Orders.Vms;
using Application.Features.Products.Vms;
using Application.Features.Users.Commands.AddOneUser;
using Application.Features.Users.Vms;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Cart;
using Domain.Entities.Order;
using Domain.ViewModels.Cart;
using Domain.ViewModels.Wishlist;
using Microsoft.AspNetCore.Identity;

namespace Application.Mapping
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductVm>().ReverseMap();
            CreateMap<User, UserVm>().ReverseMap();
            CreateMap<Order, OrderVm>().ReverseMap();

            CreateMap<User, AddOneUserCommand>().ReverseMap();
            
            CreateMap<SignUpUserCommand, User>().ReverseMap();
            CreateMap<Cart, CartViewModel>().ReverseMap();
            CreateMap<Wishlist, WishlistViewModel>().ReverseMap();
        }
    }
}