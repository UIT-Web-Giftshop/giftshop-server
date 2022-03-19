using Application.Features.Objects.Commands.Update.UpdateOneFiledOfObject;

namespace Application.Features.Users.Commands.Update.UpdateOneFiledOfUser.UpdatePasswordOfUser
{
    public class UpdatePasswordOfUserCommand : UpdateOneFieldOfObjectCommand
    {
        public string Password { get; set; }
    }
}