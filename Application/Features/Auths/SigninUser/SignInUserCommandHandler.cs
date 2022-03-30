using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Domain.Entities;
using Infrastructure.Interfaces.Repositories;
using Infrastructure.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Auths.SigninUser
{
    public class SignInUserCommandHandler : IRequestHandler<SignInUserCommand, ResponseApi<SignInResponseModel>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticationService _authenticationService;

        public SignInUserCommandHandler(IUserRepository userRepository, IAuthenticationService authenticationService)
        {
            _userRepository = userRepository;
            _authenticationService = authenticationService;
        }

        public async Task<ResponseApi<SignInResponseModel>> Handle(SignInUserCommand request, CancellationToken cancellationToken)
        {
            var existedUser = await _userRepository.GetOneAsync(q => q.Email == request.Email, cancellationToken);
            // check account is existed
            if (existedUser is null)
            {
                return ResponseApi<SignInResponseModel>.ResponseFail("Email or password is incorrect");
            }
            
            // validate password
            var validatePassword =
                new PasswordHasher<User>().VerifyHashedPassword(existedUser, existedUser.Password, request.Password);
            if (validatePassword == PasswordVerificationResult.Failed)
            {
                return ResponseApi<SignInResponseModel>.ResponseFail("Email or password is incorrect");
            }

            // generate token
            var accessToken = _authenticationService.GenerateAccessToken(existedUser);
            var returnModel = new SignInResponseModel() { AccessToken = accessToken };
            
            return ResponseApi<SignInResponseModel>.ResponseOk(returnModel);
        }
    }
}