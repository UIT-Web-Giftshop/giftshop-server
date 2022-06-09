using System.IO;

namespace Domain.Models
{
    public class ImageCheckingModel
    {
        public bool IsValid { get; set; }
        public Stream Stream { get; set; }

        public ImageCheckingModel(bool isValid, Stream stream)
        {
            IsValid = isValid;
            Stream = stream;
        }
    }
}