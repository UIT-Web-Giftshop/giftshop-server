using Application.Commons;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Images.Commands.UploadProfileAvatar
{
    public class UploadProfileAvatarCommand : IRequest<ResponseApi<Unit>>
    {
        public IFormFile File { get; set; }
    }
}