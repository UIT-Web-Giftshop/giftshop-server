using Application.Features.Auths.SignupUser;
using Application.Features.Orders.Commands.CreateOrder;
using Application.Features.Profile.Commands.UpdateProfileInfo;
using Application.Features.Users.Commands.AddOneUser;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Cart;
using Domain.Entities.User;
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

            CreateMap<UpdateProfileInfoCommand, User>()
                .ForMember(o => o.Id, opt => opt.Ignore())
                .ForMember(o => o.Email, cfg => cfg.Ignore())
                .ForMember(o => o.IsActive, cfg => cfg.Ignore())
                .ForMember(o => o.CartId, cfg => cfg.Ignore())
                .ForMember(o => o.WishlistId, cfg => cfg.Ignore());
            

            CreateMap<User, AddOneUserCommand>().ReverseMap();
            
            CreateMap<SignUpUserCommand, User>().ReverseMap();
            CreateMap<Cart, CartViewModel>().ReverseMap();
            CreateMap<Wishlist, WishlistViewModel>().ReverseMap();

            CreateMap<User, MyProfileViewModel>();

            CreateMap<Product, MinimalProductForOrder>().ReverseMap();
        }
    }
}