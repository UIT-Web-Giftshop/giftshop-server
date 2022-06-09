using Application.Commons;
using Domain.Entities;
using Domain.Entities.Account;
using MediatR;

namespace Application.Features.Users.Queries.GetOneUserById
{
    public class GetOneUserByIdQuery : IRequest<ResponseApi<User>>
    {
        public string Id { get; set; }
    }
}