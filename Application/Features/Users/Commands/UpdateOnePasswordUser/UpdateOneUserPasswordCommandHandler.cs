using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Application.Features.Users.Commands.UpdateOnePasswordUser;
using Domain.Entities;
using Domain.Entities.User;
using Infrastructure.Extensions.Mongo;
using Infrastructure.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Users.Commands.UpdateOneUser
{
    public class UpdateOneUserPasswordCommandHandler : IRequestHandler<UpdateOneUserPasswordCommand, ResponseApi<Unit>>
    {
        private readonly IUserRepository _userRepository;

        public UpdateOneUserPasswordCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ResponseApi<Unit>> Handle(UpdateOneUserPasswordCommand request, CancellationToken cancellationToken)
        {
            var existedUser = await _userRepository.GetOneAsync(request.Id, cancellationToken);
            if (existedUser is null)
            {
                return ResponseApi<Unit>.ResponseFail(ResponseConstants.ERROR_NOT_FOUND_ITEM);
            }

            existedUser.Password = new PasswordHasher<User>().HashPassword(existedUser, request.Password);
            var updated = await _userRepository.UpdateOneAsync(
                existedUser.Id,
                x => x.Set(y => y.Password, existedUser.Password),
                cancellationToken: cancellationToken);

            if (updated.AnyDocumentModified())
            {
                return ResponseApi<Unit>.ResponseOk(Unit.Value, "Cập nhập thành công");
            }
            return ResponseApi<Unit>.ResponseFail(StatusCodes.Status500InternalServerError, ResponseConstants.ERROR_EXECUTING);
        }
    }
}