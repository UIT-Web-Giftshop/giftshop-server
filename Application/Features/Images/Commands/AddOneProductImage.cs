using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Infrastructure.Extensions;
using Infrastructure.Interfaces.Repositories;
using Infrastructure.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Images.Commands
{
    public class AddOneProductImageQuery : IRequest<ResponseApi<Unit>>
    {
        public string ProductId { get; set; }
        public IFormFile File { get; set; }
    }
    
    public class AddOneProductImageHandler : IRequestHandler<AddOneProductImageQuery, ResponseApi<Unit>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IAWSS3BucketService _awsS3BucketService;

        public AddOneProductImageHandler(IProductRepository productRepository, IAWSS3BucketService awsS3BucketService)
        {
            _productRepository = productRepository;
            _awsS3BucketService = awsS3BucketService;
        }

        public async Task<ResponseApi<Unit>> Handle(AddOneProductImageQuery request, CancellationToken cancellationToken)
        {
            // check product is existed
            var product = await _productRepository.GetOneAsync(x => x.Id == request.ProductId, cancellationToken);
            if (product == null)
            {
                return ResponseApi<Unit>.ResponseFail(ResponseConstants.ERROR_NOT_FOUND_ITEM);
            }

            // check file is image
            var readFile = request.File.IsImage();
            if (!readFile.IsValid)
            {
                return ResponseApi<Unit>.ResponseFail("File is not image");
            }

            // image uid
            var prodImageUid = product.Id + "prod";
            
            // upload to s3
            var upload = await _awsS3BucketService.UploadFileAsync(
                    readFile.Stream, 
                     "products/" + prodImageUid,
                    request.File.ContentType);

            if (!upload)
            {
                return ResponseApi<Unit>.ResponseFail("Upload file fail");
            }
            
            // save image uid to product imageUrl
            var update = await _productRepository.PatchOneFieldAsync(
                x => x.Id == product.Id,
                x => x.ImageUrl,
                prodImageUid,
                cancellationToken);
            return ResponseApi<Unit>.ResponseOk(Unit.Value, "Upload file success");
        }
    }
}