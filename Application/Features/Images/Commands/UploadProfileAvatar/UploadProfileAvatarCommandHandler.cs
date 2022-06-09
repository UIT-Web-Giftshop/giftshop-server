using System;
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

namespace Application.Features.Images.Commands.UploadProfileAvatar
{
    public class UploadProfileAvatarCommandHandler : IRequestHandler<UploadProfileAvatarCommand, ResponseApi<Unit>>
    {
        private readonly IAWSS3BucketService _awsS3BucketService;
        private readonly AWSS3Settings _awss3Settings;
        private readonly IAccessorService _accessorService;
        private readonly IUserRepository _userRepository;

        public UploadProfileAvatarCommandHandler(IAWSS3BucketService awsS3BucketService, IOptions<AWSS3Settings> awss3Settings, IAccessorService accessorService, IUserRepository userRepository)
        {
            _awsS3BucketService = awsS3BucketService;
            _accessorService = accessorService;
            _userRepository = userRepository;
            _awss3Settings = awss3Settings.Value;
        }

        public async Task<ResponseApi<Unit>> Handle(UploadProfileAvatarCommand request, CancellationToken cancellationToken)
        {
            var email = _accessorService.Email();
            if (email is null)
                throw new UnauthorizedAccessException();
            
            var readFile = request.File.IsImage();
            if (!readFile.IsValid)
            {
                return ResponseApi<Unit>.ResponseFail("File không hợp lệ");
            }
            
            // image uid
            var emailName = email.Split('@');
            var imageKey = "avatar/" + emailName[0] + HashEmailToS3Uid(email);
            
            var upload = await _awsS3BucketService.UploadFileAsync(
                readFile.Stream, 
                imageKey,
                request.File.ContentType);
            
            if (!upload)
                return ResponseApi<Unit>.ResponseFail(StatusCodes.Status503ServiceUnavailable, "Lỗi khi upload file");
            
            var returnUrl = $"https://{_awss3Settings.BucketName}.s3.amazonaws.com/{imageKey}";
            
            await _userRepository.UpdateOneAsync(
                q => q.Email == email,
                x => x.Set(p => p.ImageUrl, returnUrl),
                cancellationToken: cancellationToken);

            return ResponseApi<Unit>.ResponseOk(Unit.Value, returnUrl);
        }

        private static string HashEmailToS3Uid(string email)
        {
            UInt64 hashedValue = 0;
            int i = 0;
            ulong multiplier = 1;
            while (i < email.Length)
            {
                hashedValue += email[i] * multiplier;
                multiplier *= 37;
                i++;
            }
            return hashedValue.ToString();
        }
    }
}