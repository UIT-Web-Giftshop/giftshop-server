using Application.Commons;
using Domain.Entities;
using MediatR;

namespace Application.Features.Users.Queries.GetOneUserById
{
    public class GetOneUserByIdQuery : IRequest<ResponseApi<User>>
    {
        public string Id { get; set; }
    }
}