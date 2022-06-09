using System.IO;
using System.Linq;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace Web.Tests
{
    public class MockIFormFile : Mock<IFormFile>
    {
        public MockIFormFile MockSetup(FileStream src, string type)
        {
            var fileName = src.Name.Split('/').Last();
            Setup(x => x.FileName)
                .Returns(fileName)
                .Verifiable();
            Setup(x => x.Length)
                .Returns(src.Length)
                .Verifiable();
            Setup(x => x.OpenReadStream())
                .Returns(src)
                .Verifiable();
            Setup(x => x.ContentType)
                .Returns(type)
                .Verifiable();
            return this;
        }
    }
    
    [Collection("ImageTest")]
    public class ImageFileTests
    {
        [Fact]
        [Trait("Category", "FileExtensions")]
        public void CheckFile_GivenImageFile_ReturnTrue()
        {
            // arrange
            var source = File.OpenRead(@"../../../Resources/Files/giftdemo.jpg");
            var mockFile = new MockIFormFile().MockSetup(source, "image/jpg");
            
            // act
            var result = mockFile.Object.IsImage();
            
            // assert
            Assert.True(result.IsValid);
        }

        [Fact]
        [Trait("Category", "FileExtensions")]
        public void CheckFile_GivenEmptyFile_ReturnFalse()
        {
            // arrange
            var source = File.OpenRead(@"../../../Resources/Files/giftempty.jpg");
            var mockFile = new MockIFormFile().MockSetup(source, "image/jpggg");
            
            // act
            var result = mockFile.Object.IsImage();

            // assert
            Assert.False(result.IsValid);
        }
        
        [Fact]
        [Trait("Category", "FileExtensions")]
        public void CheckFile_GivenNotImageFile_ReturnFalse()
        {
            // arrange
            var source = File.OpenRead(@"../../../Resources/Files/wrong-content.txt");
            var mockFile = new MockIFormFile().MockSetup(source, "image/png");

            // act
            var result = mockFile.Object.IsImage();

            // assert
            Assert.False(result.IsValid);
        }
    }
}