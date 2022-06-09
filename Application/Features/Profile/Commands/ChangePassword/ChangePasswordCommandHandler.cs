using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Domain.Entities;
using Domain.Entities.Account;
using Infrastructure.Extensions.Mongo;
using Infrastructure.Interfaces.Repositories;
using Infrastructure.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;

namespace Application.Features.Profile.Commands.ChangePassword
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, ResponseApi<Unit>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAccessorService _accessorService;

        public ChangePasswordCommandHandler(IAccessorService accessorService, IUserRepository userRepository)
        {
            _accessorService = accessorService;
            _userRepository = userRepository;
        }

        public async Task<ResponseApi<Unit>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var email = _accessorService.Email();
            var user = await _userRepository.FindOneAsync(x => x.Email == email, cancellationToken);

            var passwordHasher = new PasswordHasher<User>();
            var validate = passwordHasher.VerifyHashedPassword(user, user.Password, request.OldPassword);

            if (validate == PasswordVerificationResult.Failed)
            {
                return ResponseApi<Unit>.ResponseFail("Mật khẩu không chính xác");
            }

            var newPassword = passwordHasher.HashPassword(user, request.NewPassword);
            var updated = await _userRepository.UpdateOneAsync(
                user.Id,
                x => x.Set(p => p.Password, newPassword).Set(p => p.UpdatedAt, DateTime.UtcNow),
                cancellationToken: cancellationToken);

            if (!updated.AnyDocumentModified())
            {
                return ResponseApi<Unit>.ResponseFail(StatusCodes.Status500InternalServerError, ResponseConstants.ERROR_EXECUTING);   
            }

            return ResponseApi<Unit>.ResponseOk(Unit.Value);
        }
    }
}