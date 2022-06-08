using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using AutoMapper;
using Domain.Attributes;
using Domain.Entities;
using Domain.Entities.Cart;
using Domain.ViewModels.Cart;
using Infrastructure.Interfaces.Repositories;
using Infrastructure.Interfaces.Services;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Application.Features.Carts.Queries.GetOneCart
{
    public class GetOneCartByIdQueryHandler : IRequestHandler<GetOneCartQuery, ResponseApi<CartViewModel>>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;
        private readonly IAccessor _accessor;

        public GetOneCartByIdQueryHandler(ICartRepository cartRepository, IMapper mapper, IAccessor accessor)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
            _accessor = accessor;
        }

        public async Task<ResponseApi<CartViewModel>> Handle(GetOneCartQuery request, CancellationToken cancellationToken)
        {
            var lookup = new BsonDocument(
                "$lookup", 
                new BsonDocument("from", BsonCollection.GetCollectionName<Product>())
                .Add("localField", "items.sku")
                .Add("foreignField", "sku")
                .Add("as", "products"));

            var cartId = _accessor.GetHeader("cartId");
            
            if (string.IsNullOrEmpty(cartId))
                return ResponseApi<CartViewModel>.ResponseFail("cartId không tồn tại");
            
            var cart = await _cartRepository.Aggregate()
                .Match(x => x.Id == cartId)
                .AppendStage<Cart>(lookup)
                .FirstOrDefaultAsync(cancellationToken);
            
            if (cart is null)
                return ResponseApi<CartViewModel>.ResponseFail("Không tìm thấy giỏ hàng");
            
            foreach (var p in cart.Products)
            {
                foreach (var item in cart.Items.Where(item => p.Sku == item.Sku))
                {
                    p.Quantity = item.Quantity;
                    break;
                }
            }

            return ResponseApi<CartViewModel>.ResponseOk(_mapper.Map<CartViewModel>(cart));
        }
    }
}