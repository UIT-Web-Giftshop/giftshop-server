using System.Threading.Tasks;
using Domain.Models;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Interfaces.Services
{
    public interface ICloudinaryService
    {
        /// <summary>
        /// Upload image file to cloudinary
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        Task<ImageCloudinaryModel> PutImage(IFormFile file);
        /// <summary>
        /// Delete image file from cloudinary
        /// </summary>
        /// <param name="publicId"></param>
        /// <returns></returns>
        Task<string> DeleteImage(string publicId);
    }
}