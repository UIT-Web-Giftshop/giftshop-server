using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Domain.Models;
using Infrastructure.Interfaces.Repositories;
using Infrastructure.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Auths.ForgetPassword
{
    public class ForgetPasswordCommandHandler : IRequestHandler<ForgetPasswordCommand, ResponseApi<Unit>>
    {
        private readonly IVerifyTokenRepository _verifyTokenRepository;
        private readonly IMailService _mailService;
        private readonly IUserRepository _userRepository;

        public ForgetPasswordCommandHandler(IVerifyTokenRepository verifyTokenRepository, IMailService mailService, IUserRepository userRepository)
        {
            _verifyTokenRepository = verifyTokenRepository;
            _mailService = mailService;
            _userRepository = userRepository;
        }

        public async Task<ResponseApi<Unit>> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FindOneAsync(x => x.Email == request.Email, cancellationToken);

            if (user is null)
            {
                return ResponseApi<Unit>.ResponseFail("Người dùng không tồn tại");
            }

            if (!user.IsActive)
            {
                return ResponseApi<Unit>.ResponseFail("Người dùng chưa kích hoạt");
            }
            
            var nonce = Guid.NewGuid();
            var verifyToken = new Domain.Entities.VerifyToken()
            {
                Email = request.Email,
                Token = nonce.ToString(),
                Expired = DateTime.Now.AddMinutes(5),
                CreatedAt = DateTime.UtcNow
            };

            try
            {
                await _verifyTokenRepository.InsertAsync(verifyToken, cancellationToken);

                var url = $"http://localhost:5001/verify/reset-password?token={verifyToken.Token}";
                var body = $"Click this link to reset password: <a href='{url}'>{url}</a>";
                
                
                var mailRequest = new MailRequestModel()
                {
                    To = verifyToken.Email,
                    Subject = "Reset Password",
                    Body = body
                };
                await _mailService.SendAsync(mailRequest);
                
                return ResponseApi<Unit>.ResponseOk(Unit.Value, "Kiểm tra email của bạn");
            }
            catch (Exception)
            {
                return ResponseApi<Unit>.ResponseFail(StatusCodes.Status500InternalServerError, ResponseConstants.ERROR_EXECUTING);
            }
        }
    }
}