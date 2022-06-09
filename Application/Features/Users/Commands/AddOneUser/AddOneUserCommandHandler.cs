using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Account;
using Domain.Entities.Cart;
using Infrastructure.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Users.Commands.AddOneUser
{
    public class AddOneUserCommandHandler : IRequestHandler<AddOneUserCommand, ResponseApi<string>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IWishlistRepository _wishlistRepository;
        private readonly IMapper _mapper;

        public AddOneUserCommandHandler(IUserRepository userRepository, IMapper mapper, IWishlistRepository wishlistRepository, ICartRepository cartRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _wishlistRepository = wishlistRepository;
            _cartRepository = cartRepository;
        }

        public async Task<ResponseApi<string>> Handle(AddOneUserCommand request, CancellationToken cancellationToken)
        {
            var existedUser = await _userRepository.FindOneAsync(x => x.Email == request.Email, cancellationToken);
            if (existedUser is not null)
            {
                return ResponseApi<string>.ResponseFail("Email đã tồn tài");
            }

            var newUser = _mapper.Map<User>(request);
            newUser.IsActive = true;
            newUser.Role = request.Role;
            newUser.Password = new PasswordHasher<User>().HashPassword(newUser, newUser.Password);
            
            var cart = new Cart();
            var wishlist = new Wishlist();

            var addCartTask = _cartRepository.InsertAsync(cart, cancellationToken);
            var addWishlistTask = _wishlistRepository.InsertAsync(wishlist, cancellationToken);
            Task.WaitAll(addCartTask, addWishlistTask);

            newUser.CartId = cart.Id;
            newUser.WishlistId = wishlist.Id;
            
            try
            {
                await _userRepository.InsertAsync(newUser, cancellationToken);
                return ResponseApi<string>.ResponseOk(newUser.Id, StatusCodes.Status201Created, "Thêm user mới thành công");
            }
            catch (Exception e)
            {
                return ResponseApi<string>.ResponseFail(e.Message);
            }
        }
    }
}