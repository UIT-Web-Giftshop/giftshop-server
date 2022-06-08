using Application.Commons;
using MediatR;

namespace Application.Features.Profile.Commands.ForgetPassword
{
    public class ForgetPasswordCommand : IRequest<ResponseApi<Unit>>
    {
        public string Email { get; set; }
    }
}