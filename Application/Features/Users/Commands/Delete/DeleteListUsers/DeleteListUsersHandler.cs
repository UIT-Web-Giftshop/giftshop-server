using Application.Features.Objects.Commands.Delete.DeleteListObjects;
using Domain.Entities;
using Infrastructure.Interfaces.Repositories;

namespace Application.Features.Users.Commands.Delete.DeleteListUsers
{
    public class DeleteListUsersHandler : DeleteListObjectsHandler<User>
    {
        public DeleteListUsersHandler(IUserRepository _userRepository) : base(_userRepository)
        {

        }
    }
}