using System.IO;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Extensions
{
    public static class ImageExtension
    {
        /// <summary>
        /// Check contentType and extension of image
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool IsImage(this IFormFile file)
        {
            return CheckFileContentType(file) && CheckFileExtension(file);
        }

        /// <summary>
        /// Check file extension name
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private static bool CheckFileExtension(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName)!.ToLower();
            if (extension != ".jpg" &&
                extension != ".jpeg" &&
                extension != ".png" &&
                extension != ".gif" &&
                extension != ".jfif")
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Check file content type
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private static bool CheckFileContentType(IFormFile file)
        {
            var contentType = file.ContentType.ToLower();
            if (contentType != "image/jpg" &&
                contentType != "image/jpeg" &&
                contentType != "image/pjpeg" &&
                contentType != "image/gif" &&
                contentType != "image/x-png" &&
                contentType != "image/png")
            {
                return false;
            }

            return true;
        }
    }
}