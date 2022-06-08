using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using AutoMapper;
using Domain.Entities;
using Domain.ViewModels.Auth;
using Domain.ViewModels.Profile;
using Infrastructure.Interfaces.Repositories;
using Infrastructure.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Auths.SigninUser
{
    public class SignInUserCommandHandler : IRequestHandler<SignInUserCommand, ResponseApi<SignInResponseViewModel>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticationService _authenticationService;
        private readonly IMapper _mapper;
        private readonly IAccessor _accessor;

        private const string USER_NOT_FOUND = "Tài khoảng không chính xác";
        
        public SignInUserCommandHandler(IUserRepository userRepository, IAuthenticationService authenticationService, IMapper mapper, IAccessor accessor)
        {
            _userRepository = userRepository;
            _authenticationService = authenticationService;
            _mapper = mapper;
            _accessor = accessor;
        }

        public async Task<ResponseApi<SignInResponseViewModel>> Handle(SignInUserCommand request, CancellationToken cancellationToken)
        {
            var existedUser = await _userRepository.FindOneAsync(q => q.Email == request.Email, cancellationToken);
            // check account is existed
            if (existedUser is null)
            {
                return ResponseApi<SignInResponseViewModel>.ResponseFail(USER_NOT_FOUND);
            }
            
            // validate active
            if (!existedUser.IsActive)
            {
                return ResponseApi<SignInResponseViewModel>.ResponseFail("Tài khoản chưa được kích hoạt");
            }
            
            // validate password
            var validatePassword =
                new PasswordHasher<User>().VerifyHashedPassword(existedUser, existedUser.Password, request.Password);
            if (validatePassword == PasswordVerificationResult.Failed)
            {
                return ResponseApi<SignInResponseViewModel>.ResponseFail(USER_NOT_FOUND);
            }

            var updated = await _userRepository.UpdateOneAsync(
                existedUser.Id,
                x => x.Set(p => p.LastLogin, DateTime.UtcNow),
                cancellationToken: cancellationToken);

            
            // generate token
            var accessToken = _authenticationService.GenerateAccessToken(existedUser);
            _accessor.AppendSession("CartId", existedUser.CartId);
            _accessor.AppendSession("WishlistId", existedUser.WishlistId);
            
            var returnModel = new SignInResponseViewModel
            {
                AccessToken = accessToken,
                Profile = _mapper.Map<MyProfileViewModel>(existedUser)
            };
            
            return ResponseApi<SignInResponseViewModel>.ResponseOk(returnModel);
        }
    }
}