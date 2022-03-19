using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.Objects.Commands.Update.UpdateOneFiledOfObject;
using Domain.Entities;
using Infrastructure.Interfaces.Repositories;

namespace Application.Features.Users.Commands.Update.UpdateOneFiledOfUser.UpdatePasswordOfUser
{
    public class UpdatePasswordOfUserHandler : UpdateOneFieldOfObjectHandler<User,
        UpdatePasswordOfUserCommand>
    {
        public UpdatePasswordOfUserHandler(IUserRepository _userRepository) : base(_userRepository)
        {

        }

        public override async Task<bool> GetResult(Expression<Func<User, bool>> expression,
            UpdateOneFieldOfObjectCommand request, CancellationToken cancellationToken)
        {
            return await this._baseRepository.PatchOneFieldAsync(expression, p => p.Password,
                ((UpdatePasswordOfUserCommand)request).Password, cancellationToken);
        }
    }
}