using System.IO;
using MediatR;

namespace Application.Features.Images.Queries
{
    public class GetImageFromUrlQuery : IRequest<Stream>
    {
        public string Url { get; set; }
    }
}