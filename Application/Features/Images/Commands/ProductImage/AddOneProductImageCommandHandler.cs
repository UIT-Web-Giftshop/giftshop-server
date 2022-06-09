using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Domain.Settings;
using Infrastructure.Extensions;
using Infrastructure.Interfaces.Repositories;
using Infrastructure.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Application.Features.Images.Commands.ProductImage
{
    public class AddOneProductImageCommandHandler : IRequestHandler<AddOneProductImageCommand, ResponseApi<string>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IAWSS3BucketService _awsS3BucketService;
        private readonly AWSS3Settings _awss3Settings;

        public AddOneProductImageCommandHandler(IProductRepository productRepository,
            IAWSS3BucketService awsS3BucketService, IOptions<AWSS3Settings> awss3Settings)
        {
            _productRepository = productRepository;
            _awsS3BucketService = awsS3BucketService;
            _awss3Settings = awss3Settings.Value;
        }

        public async Task<ResponseApi<string>> Handle(AddOneProductImageCommand request, CancellationToken cancellationToken)
        {
            // check product is existed
            var product = await _productRepository.FindOneAsync(x => x.Sku == request.Sku, cancellationToken);
            if (product == null)
            {
                return ResponseApi<string>.ResponseFail(ResponseConstants.ERROR_NOT_FOUND_ITEM);
            }
            
            // check file is image
            var readFile = request.File.IsImage();
            if (!readFile.IsValid)
            {
                return ResponseApi<string>.ResponseFail("File không hợp lệ");
            }
            // image uid
            var imageKey = "products/" + product.Sku;
            
            // upload to s3
            var upload = await _awsS3BucketService.UploadFileAsync(
                readFile.Stream, 
                imageKey,
                request.File.ContentType);
            
            if (!upload)
            {
                return ResponseApi<string>.ResponseFail(StatusCodes.Status503ServiceUnavailable, "Lỗi khi upload file");
            }
            
            var returnUrl = $"https://{_awss3Settings.BucketName}.s3.amazonaws.com/{imageKey}";
            
            await _productRepository.UpdateOneAsync(
                product.Id,
                x => x.Set(p => p.ImageUrl, returnUrl),
                cancellationToken: cancellationToken);

            return ResponseApi<string>.ResponseOk(returnUrl);
        }
    }
}