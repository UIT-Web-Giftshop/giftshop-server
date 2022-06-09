using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Domain.Entities;
using Domain.Entities.User;
using Infrastructure.Extensions;
using Infrastructure.Extensions.Mongo;
using Infrastructure.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;

namespace Application.Features.Auths.VerifyToken.ConfirmResetPassword
{
    public class ConfirmResetPasswordCommandHandler : IRequestHandler<ConfirmResetPasswordCommand, ResponseApi<Unit>>
    {
        private readonly IVerifyTokenRepository _verifyTokenRepository;
        private readonly IUserRepository _userRepository;

        public ConfirmResetPasswordCommandHandler(IVerifyTokenRepository verifyTokenRepository,
            IUserRepository userRepository)
        {
            _verifyTokenRepository = verifyTokenRepository;
            _userRepository = userRepository;
        }

        public async Task<ResponseApi<Unit>> Handle(ConfirmResetPasswordCommand request,
            CancellationToken cancellationToken)
        {
            if (!request.Token.IsGuid())
            {
                return ResponseApi<Unit>.ResponseFail("Yêu cầu không hợp lệ");
            }

            var verifyToken =
                await _verifyTokenRepository.FindOneAsync(x => x.Token == request.Token, cancellationToken);

            if (!verifyToken.IsValid())
            {
                return ResponseApi<Unit>.ResponseFail("Yêu cầu không hợp lệ");
            }

            var user = await _userRepository.FindOneAsync(x => x.Email == verifyToken.Email, cancellationToken);

            var passwordHasher = new PasswordHasher<User>();
            var newPassword = passwordHasher.HashPassword(user, request.NewPassword);

            var updated = await _userRepository.UpdateOneAsync(
                user.Id,
                x => x.Set(p => p.Password, newPassword).Set(p => p.UpdatedAt, DateTime.UtcNow),
                cancellationToken: cancellationToken);

            if (updated.AnyDocumentModified())
            {
                await _verifyTokenRepository.DeleteOneAsync(x => x.Token == request.Token, cancellationToken);
                return ResponseApi<Unit>.ResponseOk(Unit.Value, "Đổi mật khẩu thành công");
            }
            
            return ResponseApi<Unit>.ResponseFail(StatusCodes.Status500InternalServerError, ResponseConstants.ERROR_EXECUTING);
        }
    }
}