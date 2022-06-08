using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Domain.Entities.Cart;
using Infrastructure.Extensions.Mongo;
using Infrastructure.Interfaces.Repositories;
using Infrastructure.Interfaces.Services;
using MediatR;

namespace Application.Features.Carts.Commands.UpdateOneCartItem
{
    public class UpdateAddCartItemCommandHandler : IRequestHandler<UpdateAddCartItemCommand, ResponseApi<Unit>>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IAccessorService _accessorService;

        public UpdateAddCartItemCommandHandler(ICartRepository cartRepository, IProductRepository productRepository, IAccessorService accessorService)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _accessorService = accessorService;
        }

        public async Task<ResponseApi<Unit>> Handle(UpdateAddCartItemCommand request,
            CancellationToken cancellationToken)
        {
            var cartId = _accessorService.GetHeader("cartId");
            if (string.IsNullOrEmpty(cartId))
                return ResponseApi<Unit>.ResponseFail("cartId không tồn tại");
            
            // get cart
            var cart = await _cartRepository.GetOneAsync(cartId, cancellationToken);
            if (cart is null)
                return ResponseApi<Unit>.ResponseFail((int) HttpStatusCode.BadRequest, ResponseConstants.BAD_REQUEST);

            // get product by id
            var addProduct = await _productRepository.FindOneAsync(x => x.Sku == request.Sku, cancellationToken);
            if (addProduct is null)
                return ResponseApi<Unit>.ResponseFail((int) HttpStatusCode.BadRequest, ResponseConstants.BAD_REQUEST);
            // check stock
            if (addProduct.Stock < request.Quantity)
                return ResponseApi<Unit>.ResponseFail((int) HttpStatusCode.BadRequest, "Số lượng sản phẩm không đủ");
            
            // mapping product to cart item scheme
            var addCartItem = new CartItem
            {
                Sku = request.Sku,
                Quantity = request.Quantity
            };

            // add new item to cart
            var addAffect = cart.AddItem(addCartItem);

            if (!addAffect)
                return ResponseApi<Unit>.ResponseFail((int) HttpStatusCode.InternalServerError, ResponseConstants.ERROR_EXECUTING);

            // update
            var updated = await _cartRepository.UpdateOneAsync(
                cart.Id,
                x => x.Set(y => y.Items, cart.Items),
                false,
                cancellationToken);


            if (!updated.AnyDocumentModified())
                return ResponseApi<Unit>.ResponseFail((int) HttpStatusCode.InternalServerError, ResponseConstants.ERROR_EXECUTING);
            
            return ResponseApi<Unit>.ResponseOk(Unit.Value, "Thêm sản phẩm vào giỏ hàng thành công");
        }
    }
}