using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using FluentValidation;
using Infrastructure.Interfaces.Repositories;
using Infrastructure.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Images.Commands
{
    public class AddOneProductImageQuery : IRequest<ResponseApi<string>>
    {
        public string ProductId { get; set; }
        public IFormFile File { get; set; }
    }

    public sealed class AddProductImageQueryValidator : AbstractValidator<AddOneProductImageQuery>
    {
        public AddProductImageQueryValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.File).NotNull();
        }
    }

    public class AddOneProductImageHandler : IRequestHandler<AddOneProductImageQuery, ResponseApi<string>>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICloudinaryService _cloudinaryService;

        public AddOneProductImageHandler(IProductRepository productRepository, ICloudinaryService cloudinaryService)
        {
            _productRepository = productRepository;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<ResponseApi<string>> Handle(AddOneProductImageQuery request, CancellationToken cancellationToken)
        {
            // check product is existed
            var product = await _productRepository.GetOneAsync(x => x.Id == request.ProductId, cancellationToken);
            if (product == null)
            {
                return ResponseApi<string>.ResponseFail(ResponseConstants.ERROR_NOT_FOUND_ITEM);
            }

            // Delete old image
            if (product.ImageUrl is not null)
            {
                var split = product.ImageUrl.Split('/');
                var publicId = split[^1].Split('.')[0];
                await _cloudinaryService.DeleteImage(publicId);
            }
            
            // put image to cloudinary
            // don't need to check result, because if it's fail, it will throw exception
            var putResult = await _cloudinaryService.PutImage(request.File);

            if (putResult is null)
            {
                return ResponseApi<string>.ResponseFail(StatusCodes.Status500InternalServerError, "Error in read file");
            }
            
            var affectedProductImage = _productRepository
                .PatchOneFieldAsync(x => x.Id == product.Id, x => x.ImageUrl, putResult.ImageUrl, cancellationToken);
            
            return ResponseApi<string>.ResponseOk(putResult.ImageUrl, "Add image success");
        }
    }
}