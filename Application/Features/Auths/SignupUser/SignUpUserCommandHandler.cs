using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using AutoMapper;
using Domain.Entities;
using Domain.Models;
using Infrastructure.Interfaces.Repositories;
using Infrastructure.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Auths.SignupUser
{
    public class SignUpUserCommandHandler : IRequestHandler<SignUpUserCommand, ResponseApi<Unit>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IVerifyTokenRepository _verifyTokenRepository;
        private readonly IMapper _mapper;
        private readonly IMailService _mailService;

        public SignUpUserCommandHandler(IUserRepository userRepository, IAuthenticationService authenticationService, IMapper mapper, IVerifyTokenRepository verifyTokenRepository, IMailService mailService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _verifyTokenRepository = verifyTokenRepository;
            _mailService = mailService;
        }

        public async Task<ResponseApi<Unit>> Handle(SignUpUserCommand request, CancellationToken cancellationToken)
        {
            var existedUser = await _userRepository.FindOneAsync(q => q.Email == request.Email, cancellationToken);
            if (existedUser is not null)
            {
                return ResponseApi<Unit>.ResponseFail("Email đã tồn tại");
            }

            var user = _mapper.Map<User>(request);
            user.Password = new PasswordHasher<User>().HashPassword(user, request.Password);
            user.IsActive = false;
            user.CreatedAt = DateTime.UtcNow;
            
            try
            {
                await _userRepository.InsertAsync(user, cancellationToken);

                var tokenUuid = Guid.NewGuid();
                var verifyToken = new Domain.Entities.VerifyToken()
                {
                    Email = user.Email,
                    Token = tokenUuid.ToString(),
                    CreatedAt = DateTime.UtcNow,
                    Expired = DateTime.Now.AddMinutes(10)
                };

                await _verifyTokenRepository.InsertAsync(verifyToken, cancellationToken);

                var url = $"http://localhost:5001/verify/reset-password?token={tokenUuid}";
                var mailRequest = new MailRequestModel()
                {
                    To = user.Email,
                    Subject = "Verify your account",
                    Body = $"Verify your account by click this link: <a href='{url}'>{url}</a><br>This link will be expired in 10 minutes"
                };

                await _mailService.SendAsync(mailRequest);
                return ResponseApi<Unit>.ResponseOk(Unit.Value, "Đăng ký thành công, vui lòng kiểm tra email để xác thực tài khoản");
            }
            catch (Exception)
            {
                return ResponseApi<Unit>.ResponseFail(StatusCodes.Status500InternalServerError, ResponseConstants.ERROR_EXECUTING);
            }
        }
    }
}