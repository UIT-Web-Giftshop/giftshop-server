using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Domain.ViewModels.Wishlist;
using Infrastructure.Extensions.Mongo;
using Infrastructure.Interfaces.Repositories;
using Infrastructure.Interfaces.Services;
using MediatR;

namespace Application.Features.Wishlists.Commands.UpdateOneWishlist
{
    public class UpdateAddWishlistItemCommandHandler : IRequestHandler<UpdateAddWishlistItemCommand, ResponseApi<Unit>>
    {
        private readonly IWishlistRepository _wishlistRepository;
        private readonly IProductRepository _productRepository;
        private readonly IAccessor _accessor;

        public UpdateAddWishlistItemCommandHandler(IWishlistRepository wishlistRepository, IProductRepository productRepository, IAccessor accessor)
        {
            _wishlistRepository = wishlistRepository;
            _productRepository = productRepository;
            _accessor = accessor;
        }

        public async Task<ResponseApi<Unit>> Handle(UpdateAddWishlistItemCommand request, CancellationToken cancellationToken)
        {
            var wishlistId = _accessor.GetHeader("wishlistId");
            
            if (string.IsNullOrEmpty(wishlistId))
                return ResponseApi<Unit>.ResponseFail("wishlistId không tồn tại");
            
            // get wishlist
            var wishlist = await _wishlistRepository.GetOneAsync(wishlistId, cancellationToken);
            if (wishlist is null)
                return ResponseApi<Unit>.ResponseFail("Không tìm thấy wishlist");

            // get product by sku
            var product = await _productRepository.FindOneAsync(x => x.Sku == request.Sku, cancellationToken);
            if (product is null)
                return ResponseApi<Unit>.ResponseFail("Không tìm thấy sản phẩm");

            // add wishlist
            var added = wishlist.AddItem(product.Sku);
            if (!added)
                return ResponseApi<Unit>.ResponseFail("Sản phẩm đã tồn tại trong wishlist");
            
            // update wishlist
            var updated = await _wishlistRepository.UpdateOneAsync(
                wishlist.Id,
                x => x.Set(y => y.Items, wishlist.Items),
                true,
                cancellationToken);
            
            if (!updated.AnyDocumentModified())
                return ResponseApi<Unit>.ResponseFail((int) HttpStatusCode.InternalServerError, ResponseConstants.ERROR_EXECUTING);
            
            return ResponseApi<Unit>.ResponseOk(Unit.Value, "Thêm sản phẩm vào wishlist thành công");

        }
    }
}