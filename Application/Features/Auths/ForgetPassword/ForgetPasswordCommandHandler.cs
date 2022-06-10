using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Domain.Models;
using Domain.Settings;
using Infrastructure.Interfaces.Repositories;
using Infrastructure.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using RazorEmailLibs.Constants;
using RazorEmailLibs.Views.Emails;

namespace Application.Features.Auths.ForgetPassword
{
    public class ForgetPasswordCommandHandler : IRequestHandler<ForgetPasswordCommand, ResponseApi<Unit>>
    {
        private readonly IVerifyTokenRepository _verifyTokenRepository;
        private readonly IMailService _mailService;
        private readonly IUserRepository _userRepository;
        private readonly DomainSettings _domainSettings;

        public ForgetPasswordCommandHandler(IVerifyTokenRepository verifyTokenRepository, IMailService mailService, IUserRepository userRepository, IOptions<DomainSettings> domainSettings)
        {
            _verifyTokenRepository = verifyTokenRepository;
            _mailService = mailService;
            _userRepository = userRepository;
            _domainSettings = domainSettings.Value;
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
                var url = $"{_domainSettings.DomainName}/quen-mat-khau?token={verifyToken.Token}";
                var mailModel = new ResetPasswordViewModel(url);
                await _mailService.SendWithTemplate(user.Email, "Reset password", new List<IFormFile>(),
                    MailTemplatesName.RESET_ACCOUNT_PASSWORD, mailModel);
                
                return ResponseApi<Unit>.ResponseOk(Unit.Value, "Kiểm tra email của bạn");
            }
            catch (Exception)
            {
                return ResponseApi<Unit>.ResponseFail(StatusCodes.Status500InternalServerError, ResponseConstants.ERROR_EXECUTING);
            }
        }
    }
}