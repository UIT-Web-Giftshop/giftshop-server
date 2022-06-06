using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Infrastructure.Extensions.Mongo;
using Infrastructure.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Wishlists.Commands.UpdateOneWishlist
{
    public class UpdateDeleteWishlistItemCommandHandler : IRequestHandler<UpdateDeleteWishlistItemCommand, ResponseApi<Unit>>
    {
        private readonly IWishlistRepository _wishlistRepository;

        public UpdateDeleteWishlistItemCommandHandler(IWishlistRepository wishlistRepository)
        {
            _wishlistRepository = wishlistRepository;
        }

        public async Task<ResponseApi<Unit>> Handle(UpdateDeleteWishlistItemCommand request, CancellationToken cancellationToken)
        {
            var wishlist = await _wishlistRepository.GetOneAsync(request.Id, cancellationToken);
            if (wishlist is null)
                return ResponseApi<Unit>.ResponseFail("Không tìm thấy wishlist");

            var removed = wishlist.RemoveItem(request.Sku);
            if (!removed)
                return ResponseApi<Unit>.ResponseFail("Không tìm thấy item");
            
            var updated = await _wishlistRepository.UpdateOneAsync(
                wishlist.Id,
                x => x.Set(y => y.Items, wishlist.Items),
                true,
                cancellationToken);
            
            if (!updated.AnyDocumentModified())
                return ResponseApi<Unit>.ResponseFail((int) HttpStatusCode.InternalServerError, ResponseConstants.ERROR_EXECUTING);
            
            return ResponseApi<Unit>.ResponseOk(Unit.Value,"Cập nhật thành công");
        }
    }
}