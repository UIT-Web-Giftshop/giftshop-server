using Application.Features.Objects.Commands.Delete.DeleteOneObject;
using Domain.Entities;

namespace Application.Features.Users.Commands.Delete.DeleteOneUser
{
    public abstract class DeleteOneUserCommand : DeleteOneObjectCommand<User>
    {

    }
}