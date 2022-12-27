using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using AutoMapper;
using Domain.Entities.Account;
using Domain.Settings;
using Infrastructure.Interfaces.Repositories;
using Infrastructure.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using RazorEmailLibs.Constants;
using RazorEmailLibs.Views.Emails;

namespace Application.Features.Auths.SignupUser
{
	public class SignUpUserCommandHandler : IRequestHandler<SignUpUserCommand, ResponseApi<Unit>>
	{
		private readonly DomainSettings _domainSettings;
		private readonly IMailService _mailService;
		private readonly IMapper _mapper;
		private readonly IUserRepository _userRepository;
		private readonly IVerifyTokenRepository _verifyTokenRepository;

		public SignUpUserCommandHandler(
			IUserRepository userRepository,
			IAuthenticationService authenticationService,
			IMapper mapper,
			IVerifyTokenRepository verifyTokenRepository,
			IMailService mailService,
			IOptions<DomainSettings> domainSettings)
		{
			_userRepository = userRepository;
			_mapper = mapper;
			_verifyTokenRepository = verifyTokenRepository;
			_mailService = mailService;
			_domainSettings = domainSettings.Value;
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
			user.Role = nameof(UserRoles.CLIENT);

			try
			{
				await _userRepository.InsertAsync(user, cancellationToken);

				var tokenUuid = Guid.NewGuid();
				var verifyToken = new Domain.Entities.VerifyToken
				{
					Email = user.Email,
					Token = tokenUuid.ToString(),
					CreatedAt = DateTime.UtcNow,
					Expired = DateTime.Now.AddMinutes(10)
				};

				await _verifyTokenRepository.InsertAsync(verifyToken, cancellationToken);

				var mailModel = new ConfirmAccountEmailViewModel(
					$"{_domainSettings.ForwardUrl}/xac-nhan-email?token={tokenUuid}",
					$"{_domainSettings.ForwardUrl}/api/me/resend-confirm-email?email={user.Email}");

				await _mailService.SendWithTemplate(user.Email, "Confirm Email Giftshop", new List<IFormFile>(),
					MailTemplatesName.CONFIRM_ACCOUNT_EMAIL, mailModel);

				return ResponseApi<Unit>.ResponseOk(Unit.Value,
					"Đăng ký thành công, vui lòng kiểm tra email để xác thực tài khoản");
			}
			catch (Exception)
			{
				return ResponseApi<Unit>.ResponseFail(StatusCodes.Status500InternalServerError,
					ResponseConstants.ERROR_EXECUTING);
			}
		}
	}
}
