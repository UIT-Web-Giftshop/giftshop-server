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
    public class UpdateDeleteCartItemCommandHandler : IRequestHandler<UpdateDeleteCartItemCommand, ResponseApi<Unit>>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IAccessor _accessor;
        
        public UpdateDeleteCartItemCommandHandler(ICartRepository cartRepository, IAccessor accessor)
        {
            _cartRepository = cartRepository;
            _accessor = accessor;
        }
        
        public async Task<ResponseApi<Unit>> Handle(UpdateDeleteCartItemCommand request, CancellationToken cancellationToken)
        {
            var cartId = _accessor.GetHeader("cartId");
            
            if (string.IsNullOrEmpty(cartId))
                return ResponseApi<Unit>.ResponseFail("cartId không tồn tại");
            
            var cart = await _cartRepository.GetOneAsync(cartId, cancellationToken);
            if (cart == null)
                return ResponseApi<Unit>.ResponseFail((int) HttpStatusCode.BadGateway, "Không tìm thấy cart");

            var affected =
                cart.RemoveItem(new CartItem { Sku = request.Sku, Quantity = request.Quantity });
            
            if (!affected)
                return ResponseApi<Unit>.ResponseFail((int) HttpStatusCode.BadRequest, "Lỗi không thể cập nhật giỏ hàng");
            
            var updated = await _cartRepository.UpdateOneAsync(
                cart.Id,
                x => x.Set(y => y.Items, cart.Items),
                false,
                cancellationToken);
            
            if (!updated.AnyDocumentModified())
                return ResponseApi<Unit>.ResponseFail((int) HttpStatusCode.InternalServerError, ResponseConstants.ERROR_EXECUTING);
            
            return ResponseApi<Unit>.ResponseOk(Unit.Value, "Cập nhật giỏ hàng thành công");
        }
    }
}