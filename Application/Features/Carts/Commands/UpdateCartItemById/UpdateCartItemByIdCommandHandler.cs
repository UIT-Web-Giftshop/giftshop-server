using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Domain.Attributes;
using Domain.Entities.Cart;
using Infrastructure.Extensions;
using Infrastructure.Extensions.Mongo;
using Infrastructure.Interfaces.Repositories;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Application.Features.Carts.Commands.UpdateCartItemById
{
    public class UpdateCartItemByIdCommandHandler : IRequestHandler<UpdateCartItemByIdCommand, ResponseApi<Unit>>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public UpdateCartItemByIdCommandHandler(ICartRepository cartRepository, IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        public async Task<ResponseApi<Unit>> Handle(UpdateCartItemByIdCommand request,
            CancellationToken cancellationToken)
        {
            // get cart
            var cart = await _cartRepository.GetOneAsync(request.Id, cancellationToken);
            if (cart is null)
                return ResponseApi<Unit>.ResponseFail((int) HttpStatusCode.BadRequest, ResponseConstants.BAD_REQUEST);

            // get product by id
            var addProduct = await _productRepository.GetOneAsync(x => x.Id == request.ProductId, cancellationToken);
            if (addProduct is null)
                return ResponseApi<Unit>.ResponseFail((int) HttpStatusCode.BadRequest, ResponseConstants.BAD_REQUEST);
            // check stock
            if (addProduct.Stock < request.Quantity)
                return ResponseApi<Unit>.ResponseFail((int) HttpStatusCode.BadRequest, "Số lượng sản phẩm không đủ");
            
            // mapping product to cart item scheme
            var addCartItem = new CartItem
            {
                ProductId = new ObjectId(request.ProductId),
                Quantity = request.Quantity
            };

            // add new item to cart
            var addAffect = cart.AddItem(addCartItem);

            if (!addAffect)
                return ResponseApi<Unit>.ResponseFail((int) HttpStatusCode.InternalServerError, ResponseConstants.ERROR_EXECUTING);

            // update
            var result = await _cartRepository.UpdateOneAsync(
                cart.Id,
                x => x.Set(y => y.Items, cart.Items),
                true,
                cancellationToken);


            // return response
            return result.AnyDocumentModified()
                ? ResponseApi<Unit>.ResponseOk(Unit.Value, "Add item to cart success")
                : ResponseApi<Unit>.ResponseFail((int) HttpStatusCode.InternalServerError,
                    ResponseConstants.ERROR_EXECUTING);
        }
    }
}