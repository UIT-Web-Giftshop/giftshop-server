using System.IO;

namespace Domain.Models
{
    public class ImageExtensionModel
    {
        public bool IsValid { get; set; }
        public Stream Stream { get; set; }

        public ImageExtensionModel(bool isValid, Stream stream)
        {
            IsValid = isValid;
            Stream = stream;
        }
    }
}