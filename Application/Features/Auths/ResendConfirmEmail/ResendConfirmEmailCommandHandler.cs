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

namespace Application.Features.Auths.ResendConfirmEmail
{
    public class ResendConfirmEmailCommandHandler : IRequestHandler<ResendConfirmEmailCommand, ResponseApi<Unit>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IVerifyTokenRepository _verifyTokenRepository;
        private readonly IMailService _mailService;
        private readonly DomainSettings _domainSettings;

        public ResendConfirmEmailCommandHandler(
            IUserRepository userRepository,
            IVerifyTokenRepository verifyTokenRepository,
            IMailService mailService,
            IOptions<DomainSettings> domainSettings)
        {
            _userRepository = userRepository;
            _verifyTokenRepository = verifyTokenRepository;
            _mailService = mailService;
            _domainSettings = domainSettings.Value;
        }

        public async Task<ResponseApi<Unit>> Handle(ResendConfirmEmailCommand request,
            CancellationToken cancellationToken)
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

            var mailModel = new ConfirmAccountEmailViewModel(
                $"{_domainSettings.DomainName}/xac-nhan-email?token={tokenUuid}",
                $"{_domainSettings.DomainName}/api/me/resend-confirm-email?email={user.Email}");

            try
            {
                await _verifyTokenRepository.InsertAsync(verifyToken, cancellationToken);
                await _mailService.SendWithTemplate(user.Email, "Confirm Email", new List<IFormFile>(),
                    MailTemplatesName.CONFIRM_ACCOUNT_EMAIL, mailModel);
                return ResponseApi<Unit>.ResponseOk(Unit.Value, "Đã gửi email xác nhận");
            }
            catch (Exception)
            {
                return ResponseApi<Unit>.ResponseFail(ResponseConstants.ERROR_EXECUTING);
            }
        }
    }
}