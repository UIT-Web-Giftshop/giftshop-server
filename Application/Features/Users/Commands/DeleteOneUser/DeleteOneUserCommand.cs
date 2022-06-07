using Application.Commons;
using MediatR;

namespace Application.Features.Users.Commands.DeleteOneUser
{
    public class DeleteOneUserCommand : IRequest<ResponseApi<Unit>>
    {
        public string Id { get; set; }
    }
}