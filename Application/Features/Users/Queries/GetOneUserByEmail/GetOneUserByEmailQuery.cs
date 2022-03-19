using Application.Commons;
using Application.Features.Objects.Queries.GetOneObject;
using Application.Features.Users.Vms;
using MediatR;

namespace Application.Features.Users.Queries.GetOneUserByEmail
{
    public class GetOneUserByEmailQuery : GetOneObjectQuery, IRequest<ResponseApi<UserVm>>
    {
        public string Email { get; set; }
    }
}