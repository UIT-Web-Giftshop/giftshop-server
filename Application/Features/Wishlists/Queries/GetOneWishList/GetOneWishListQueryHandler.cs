using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using AutoMapper;
using Domain.Attributes;
using Domain.Entities;
using Domain.ViewModels.Wishlist;
using Infrastructure.Interfaces.Repositories;
using Infrastructure.Interfaces.Services;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Application.Features.Wishlists.Queries.GetOneWishList
{
    public class GetOneWishListQueryHandler : IRequestHandler<GetOneWishlistQuery, ResponseApi<WishlistViewModel>>
    {
        private readonly IWishlistRepository _wishlistRepository;
        private readonly IMapper _mapper;
        private readonly IAccessorService _accessorService;
        
        public GetOneWishListQueryHandler(IWishlistRepository wishlistRepository, IMapper mapper, IAccessorService accessorService)
        {
            _wishlistRepository = wishlistRepository;
            _mapper = mapper;
            _accessorService = accessorService;
        }

        public async Task<ResponseApi<WishlistViewModel>> Handle(GetOneWishlistQuery request, CancellationToken cancellationToken)
        {
            var wishlistId = _accessorService.GetHeader("wishlistId");
            
            if (string.IsNullOrEmpty(wishlistId))
                return ResponseApi<WishlistViewModel>.ResponseFail("wishlistId không tồn tại");
            
            var lookup = new BsonDocument(
                "$lookup", 
                new BsonDocument("from", BsonCollection.GetCollectionName<Product>())
                    .Add("localField", "items")
                    .Add("foreignField", "sku")
                    .Add("as", "products"));
            
            var wishlist = await _wishlistRepository.Aggregate()
                .Match(x => x.Id == wishlistId)
                .AppendStage<Domain.Entities.Wishlist>(lookup)
                .FirstOrDefaultAsync(cancellationToken);
            
            if (wishlist is null)
                return ResponseApi<WishlistViewModel>.ResponseFail("Không tìm thấy wishlist");
            
            return ResponseApi<WishlistViewModel>.ResponseOk(_mapper.Map<WishlistViewModel>(wishlist));
        }
    }
}