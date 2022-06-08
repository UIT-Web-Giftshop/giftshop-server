using Application.Features.Auths.SignupUser;
using Application.Features.Orders.Vms;
using Application.Features.Users.Commands.AddOneUser;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Cart;
using Domain.Entities.Order;
using Domain.ViewModels.Cart;
using Domain.ViewModels.Product;
using Domain.ViewModels.Profile;
using Domain.ViewModels.Wishlist;

namespace Application.Mapping
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<ProductDetailViewModel, Product>()
                .ForMember(o => o.Id, opt => opt.Ignore())
                .ForMember(o => o.Sku, cfg => cfg.Ignore());
                // .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember is not null));
            CreateMap<Product, ProductDetailViewModel>();
            
            CreateMap<Order, OrderVm>().ReverseMap();

            CreateMap<User, AddOneUserCommand>().ReverseMap();
            
            CreateMap<SignUpUserCommand, User>().ReverseMap();
            CreateMap<Cart, CartViewModel>().ReverseMap();
            CreateMap<Wishlist, WishlistViewModel>().ReverseMap();

            CreateMap<User, MyProfileViewModel>();
        }
    }
}