using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Application.Features.Wishlist.Queries.GetOneWishList;
using AutoMapper;
using Domain.Attributes;
using Domain.Entities;
using Domain.ViewModels.Wishlist;
using Infrastructure.Interfaces.Repositories;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Application.Features.Wishlists.Queries.GetOneWishList
{
    public class GetOneWishListQueryHandler : IRequestHandler<GetOneWishlistQuery, ResponseApi<WishlistViewModel>>
    {
        private readonly IWishlistRepository _wishlistRepository;
        private readonly IMapper _mapper;

        public GetOneWishListQueryHandler(IWishlistRepository wishlistRepository, IMapper mapper)
        {
            _wishlistRepository = wishlistRepository;
            _mapper = mapper;
        }

        public async Task<ResponseApi<WishlistViewModel>> Handle(GetOneWishlistQuery request, CancellationToken cancellationToken)
        {
            var lookup = new BsonDocument(
                "$lookup", 
                new BsonDocument("from", BsonCollection.GetCollectionName<Product>())
                    .Add("localField", "items")
                    .Add("foreignField", "sku")
                    .Add("as", "products"));
            
            var wishlist = await _wishlistRepository.Aggregate()
                .Match(x => x.Id == request.Id)
                .AppendStage<Domain.Entities.Wishlist>(lookup)
                .FirstOrDefaultAsync(cancellationToken);
            
            if (wishlist is null)
                return ResponseApi<WishlistViewModel>.ResponseFail("Không tìm thấy wishlist");
            
            return ResponseApi<WishlistViewModel>.ResponseOk(_mapper.Map<WishlistViewModel>(wishlist));
        }
    }
}