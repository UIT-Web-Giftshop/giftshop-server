using System;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Domain.Models;
using Domain.Settings;
using Infrastructure.Extensions;
using Infrastructure.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IOptions<CloudinarySettings> options)
        {
            var account = new Account(
                options.Value.CloudName,
                options.Value.ApiKey,
                options.Value.ApiSecret);
            _cloudinary = new Cloudinary(account);
        }

        public async Task<ImageCloudinaryModel> PutImage(IFormFile file)
        {
            // read & check file
            var check = file.IsImage();
            if (!check.IsValid)
                return null;

            var uploadParam = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, file.OpenReadStream())
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParam);
            if (uploadResult.Error is not null)
            {
                throw new Exception(uploadResult.Error.Message);
            }

            return new ImageCloudinaryModel()
            {
                PublicId = uploadResult.PublicId,
                ImageUrl = uploadResult.SecureUrl.ToString()
            };
        }

        public async Task<string> DeleteImage(string publicId)
        {
            var destroyParam = new DeletionParams(publicId);
            var deleteResult = await _cloudinary.DestroyAsync(destroyParam);
            return deleteResult.Result == "ok" ? deleteResult.Result : null;
        }
    }
}