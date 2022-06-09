using Application.Commons;
using Domain.Entities;
using Domain.Entities.Account;
using MediatR;

namespace Application.Features.Users.Queries.GetOneUserByEmail
{
    public class GetOneUserByEmailQuery : IRequest<ResponseApi<User>>
    {
        public string Email { get; set; }
    }
}