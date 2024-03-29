﻿using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Domain.Models;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Extensions
{
    public static class ImageExtension
    {
        private const int IMAGE_MINIMUM_BYTES = 512;
        
        /// <summary>
        /// Check contentType and extension of image
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static ImageCheckingModel IsImage(this IFormFile file)
        {
            var failure = new ImageCheckingModel(false, Stream.Null);

            if (!CheckFileContentType(file) || !CheckFileExtension(file))
                return failure;
            
            // try to read the file and check the first byte
            try
            {
                var stream = file.OpenReadStream();
                // open stream
                if (!stream.CanRead)
                {
                    return failure;
                }
                // check image size
                if (file.Length < IMAGE_MINIMUM_BYTES)
                {
                    return failure;
                }
                
                // read file content & check pattern
                byte[] buffer = new byte[IMAGE_MINIMUM_BYTES];
                stream.Read(buffer, 0, IMAGE_MINIMUM_BYTES);
                var content = Encoding.UTF8.GetString(buffer);
                if (Regex.IsMatch(
                        content, 
                        @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
                {
                    return failure;
                }

                return new ImageCheckingModel(true, stream);
            }
            catch (Exception)
            {
                return failure;
            }
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