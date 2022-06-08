using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Infrastructure.Extensions.Mongo;
using Infrastructure.Interfaces.Repositories;
using Infrastructure.Interfaces.Services;
using MediatR;

namespace Application.Features.Wishlists.Commands.UpdateOneWishlist
{
    public class UpdateDeleteWishlistItemCommandHandler : IRequestHandler<UpdateDeleteWishlistItemCommand, ResponseApi<Unit>>
    {
        private readonly IWishlistRepository _wishlistRepository;
        private readonly IAccessor _accessor;

        public UpdateDeleteWishlistItemCommandHandler(IWishlistRepository wishlistRepository, IAccessor accessor)
        {
            _wishlistRepository = wishlistRepository;
            _accessor = accessor;
        }

        public async Task<ResponseApi<Unit>> Handle(UpdateDeleteWishlistItemCommand request, CancellationToken cancellationToken)
        {
            var wishlistId = _accessor.GetHeader("wishlistId");
            
            if (string.IsNullOrEmpty(wishlistId))
                return ResponseApi<Unit>.ResponseFail("wishlistId không tồn tại");
            
            var wishlist = await _wishlistRepository.GetOneAsync(wishlistId, cancellationToken);
            if (wishlist is null)
                return ResponseApi<Unit>.ResponseFail("Không tìm thấy wishlist");

            var removed = wishlist.RemoveItem(request.Sku);
            if (!removed)
                return ResponseApi<Unit>.ResponseFail("Không tìm thấy item");
            
            var updated = await _wishlistRepository.UpdateOneAsync(
                wishlist.Id,
                x => x.Set(y => y.Items, wishlist.Items),
                false,
                cancellationToken);
            
            if (!updated.AnyDocumentModified())
                return ResponseApi<Unit>.ResponseFail((int) HttpStatusCode.InternalServerError, ResponseConstants.ERROR_EXECUTING);
            
            return ResponseApi<Unit>.ResponseOk(Unit.Value,"Cập nhật thành công");
        }
    }
}