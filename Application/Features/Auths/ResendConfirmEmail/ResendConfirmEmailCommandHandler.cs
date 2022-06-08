using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Domain.Models;
using Infrastructure.Interfaces.Repositories;
using Infrastructure.Interfaces.Services;
using MediatR;

namespace Application.Features.Auths.ResendConfirmEmail
{
    public class ResendConfirmEmailCommandHandler : IRequestHandler<ResendConfirmEmailCommand, ResponseApi<Unit>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IVerifyTokenRepository _verifyTokenRepository;
        private readonly IMailService _mailService;

        public ResendConfirmEmailCommandHandler(IUserRepository userRepository, IVerifyTokenRepository verifyTokenRepository, IMailService mailService)
        {
            _userRepository = userRepository;
            _verifyTokenRepository = verifyTokenRepository;
            _mailService = mailService;
        }

        public async Task<ResponseApi<Unit>> Handle(ResendConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FindOneAsync(x => x.Email == request.Email, cancellationToken);
            
            if (user is null)
            {
                return ResponseApi<Unit>.ResponseFail("Người dùng không tồn tại");
            }

            if (user.IsActive)
            {
                return ResponseApi<Unit>.ResponseFail("Người dùng đã được kích hoạt");
            }

            var tokenUuid = Guid.NewGuid();
            var verifyToken = new Domain.Entities.VerifyToken()
            {
                Email = user.Email,
                Token = tokenUuid.ToString(),
                CreatedAt = DateTime.UtcNow,
                Expired = DateTime.Now.AddMinutes(10)
            };
            
            var url = $"http://localhost:5001/verify/reset-password?token={tokenUuid}";
            var mailRequest = new MailRequestModel()
            {
                To = user.Email,
                Subject = "Verify your account",
                Body = $"Verify your account by click this link: <a href='{url}'>{url}</a><br>This link will be expired in 10 minutes"
            };
            
            try
            {
                await _verifyTokenRepository.InsertAsync(verifyToken, cancellationToken);
                await _mailService.SendAsync(mailRequest);
                return ResponseApi<Unit>.ResponseOk(Unit.Value, "Đã gửi email xác nhận");
            }
            catch (Exception)
            {
                return ResponseApi<Unit>.ResponseFail(ResponseConstants.ERROR_EXECUTING);
            }
        }
    }
}