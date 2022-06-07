using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Domain.Entities.Cart;
using Infrastructure.Extensions.Mongo;
using Infrastructure.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Carts.Commands.UpdateOneCartItem
{
    public class UpdateAddCartItemCommandHandler : IRequestHandler<UpdateAddCartItemCommand, ResponseApi<Unit>>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public UpdateAddCartItemCommandHandler(ICartRepository cartRepository, IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        public async Task<ResponseApi<Unit>> Handle(UpdateAddCartItemCommand request,
            CancellationToken cancellationToken)
        {
            // get cart
            var cart = await _cartRepository.GetOneAsync(request.Id, cancellationToken);
            if (cart is null)
                return ResponseApi<Unit>.ResponseFail((int) HttpStatusCode.BadRequest, ResponseConstants.BAD_REQUEST);

            // get product by id
            var addProduct = await _productRepository.GetOneAsync(x => x.Sku == request.Sku, cancellationToken);
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