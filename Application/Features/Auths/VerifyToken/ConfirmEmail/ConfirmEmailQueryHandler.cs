using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Domain.Entities;
using Domain.Entities.Cart;
using Infrastructure.Extensions.Mongo;
using Infrastructure.Interfaces.Repositories;
using MediatR;
using MongoDB.Driver;

namespace Application.Features.Auths.VerifyToken.ConfirmEmail
{
    public class ConfirmEmailQueryHandler : IRequestHandler<ConfirmEmailQuery, ResponseApi<Unit>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IVerifyTokenRepository _verifyTokenRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IWishlistRepository _wishlistRepository;

        public ConfirmEmailQueryHandler(IUserRepository userRepository, IVerifyTokenRepository verifyTokenRepository, ICartRepository cartRepository, IWishlistRepository wishlistRepository)
        {
            _userRepository = userRepository;
            _verifyTokenRepository = verifyTokenRepository;
            _cartRepository = cartRepository;
            _wishlistRepository = wishlistRepository;
        }

        public async Task<ResponseApi<Unit>> Handle(ConfirmEmailQuery request, CancellationToken cancellationToken)
        {
            Guid.TryParse(request.Token, out var token);
            
            if (token == Guid.Empty)
                return ResponseApi<Unit>.ResponseFail("Yêu cầu không hợp lệ");
            
            var verifyToken =
                await _verifyTokenRepository.FindOneAsync(x => x.Token == request.Token, cancellationToken);

            if (verifyToken is null)
            {
                return ResponseApi<Unit>.ResponseFail("Yêu cầu không hợp lệ");
            }
            
            var user = await _userRepository.FindOneAsync(x => x.Email == verifyToken.Email, cancellationToken);

            var cart = new Cart();
            var wishlist = new Wishlist();

            var addCartTask = _cartRepository.InsertAsync(cart, cancellationToken);
            var addWishlistTask = _wishlistRepository.InsertAsync(wishlist, cancellationToken);
            
            await Task.WhenAll(addCartTask, addWishlistTask);
            
            // update user
            var updated = await _userRepository.UpdateOneAsync(
                user.Id,
                x => x.Set(p => p.IsActive, true).Set(p => p.CartId, cart.Id).Set(p => p.WishlistId, wishlist.Id),
                cancellationToken: cancellationToken);

            if (updated.AnyDocumentModified())
            {
                return ResponseApi<Unit>.ResponseOk(Unit.Value);
            }

            return ResponseApi<Unit>.ResponseFail(ResponseConstants.ERROR_EXECUTING);
        }
    }
}