using Application.Features.Objects.Commands.Delete.DeleteOneObject;
using Domain.Entities;
using Infrastructure.Interfaces.Repositories;

namespace Application.Features.Users.Commands.Delete.DeleteOneUser
{
    public class DeleteOneUserHandler : DeleteOneObjectHandler<User>
    {
        public DeleteOneUserHandler(IUserRepository _userRepository) : base(_userRepository)
        {

        }
    }
}