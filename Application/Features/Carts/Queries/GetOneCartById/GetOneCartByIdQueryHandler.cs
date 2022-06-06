using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using AutoMapper;
using Domain.Attributes;
using Domain.Entities;
using Domain.Entities.Cart;
using Domain.ViewModels.Cart;
using Infrastructure.Interfaces.Repositories;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Application.Features.Carts.Queries.GetOneCartById
{
    public class GetOneCartByIdQueryHandler : IRequestHandler<GetOneCartByIdQuery, ResponseApi<CartViewModel>>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public GetOneCartByIdQueryHandler(ICartRepository cartRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
        }

        public async Task<ResponseApi<CartViewModel>> Handle(GetOneCartByIdQuery request, CancellationToken cancellationToken)
        {
            var lookup = new BsonDocument(
                "$lookup", 
                new BsonDocument("from", BsonCollection.GetCollectionName<Product>())
                .Add("localField", "items.productId")
                .Add("foreignField", "_id")
                .Add("as", "products"));

            var cart = await _cartRepository.Aggregate()
                .Match(x => x.Id == request.Id)
                .AppendStage<Cart>(lookup)
                .FirstOrDefaultAsync(cancellationToken);
            
            if (cart is null)
                return ResponseApi<CartViewModel>.ResponseFail("No cart found");
            
            var index = 0;
            foreach (var item in cart.Items)
            {
                cart.Products[index++].Quantity = item.Quantity;
            }

            return ResponseApi<CartViewModel>.ResponseOk(_mapper.Map<CartViewModel>(cart));
        }
    }
}