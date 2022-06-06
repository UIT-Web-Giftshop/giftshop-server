using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Infrastructure.Extensions.Mongo;
using Infrastructure.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Wishlists.Commands.UpdateOneWishlist
{
    public class UpdateAddWishlistItemCommandHandler : IRequestHandler<UpdateAddWishlistItemCommand, ResponseApi<Unit>>
    {
        private readonly IWishlistRepository _wishlistRepository;
        private readonly IProductRepository _productRepository;

        public UpdateAddWishlistItemCommandHandler(IWishlistRepository wishlistRepository, IProductRepository productRepository)
        {
            _wishlistRepository = wishlistRepository;
            _productRepository = productRepository;
        }

        public async Task<ResponseApi<Unit>> Handle(UpdateAddWishlistItemCommand request, CancellationToken cancellationToken)
        {
            // get wishlist
            var wishlist = await _wishlistRepository.GetOneAsync(request.Id, cancellationToken);
            if (wishlist is null)
                return ResponseApi<Unit>.ResponseFail("Không tìm thấy wishlist");

            // get product by sku
            var product = await _productRepository.GetOneAsync(x => x.Sku == request.Sku, cancellationToken);
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