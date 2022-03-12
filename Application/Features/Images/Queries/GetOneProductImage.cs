using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using FluentValidation;
using Infrastructure.Interfaces.Services;
using MediatR;

namespace Application.Features.Images.Queries
{
    public class GetOneProductImageQuery : IRequest<ResponseApi<Stream>>
    {
        public string Key { get; set; }
    }

    public class GetOneProductImageValidator : AbstractValidator<GetOneProductImageQuery>
    {
        public GetOneProductImageValidator()
        {
            RuleFor(x => x.Key).NotEmpty();
        }
    }
    
    public class GetOneProductImageHandler : IRequestHandler<GetOneProductImageQuery, ResponseApi<Stream>>
    {
        private readonly IAWSS3BucketService _awsS3BucketService;

        public GetOneProductImageHandler(IAWSS3BucketService awsS3BucketService)
        {
            _awsS3BucketService = awsS3BucketService;
        }

        public async Task<ResponseApi<Stream>> Handle(GetOneProductImageQuery request, CancellationToken cancellationToken)
        {
            var data = await _awsS3BucketService.GetFileAsync( "products/" + request.Key);
            if (data == Stream.Null)
            {
                return ResponseApi<Stream>.ResponseFail(ResponseConstants.ERROR_NOT_FOUND_ITEM);
            }
            return ResponseApi<Stream>.ResponseOk(data);
        }
    }
}