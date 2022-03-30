using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces.Repositories;
using Infrastructure.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Auths.SignupUser
{
    public class SignupUserCommandHandler : IRequestHandler<SignupUserCommand, ResponseApi<Unit>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticationService _authenticationService;
        private readonly IMapper _mapper;

        public SignupUserCommandHandler(IUserRepository userRepository, IAuthenticationService authenticationService, IMapper mapper)
        {
            _userRepository = userRepository;
            _authenticationService = authenticationService;
            _mapper = mapper;
        }

        public async Task<ResponseApi<Unit>> Handle(SignupUserCommand request, CancellationToken cancellationToken)
        {
            var existedUser = await _userRepository.GetOneAsync(q => q.Email == request.Email, cancellationToken);
            if (existedUser is not null)
            {
                return ResponseApi<Unit>.ResponseFail("Email is existed");
            }

            var user = _mapper.Map<User>(request);
            user.Password = new PasswordHasher<User>().HashPassword(user, request.Password);
            user.CreatedAt = DateTime.UtcNow;
            
            try
            {
                await _userRepository.AddAsync(user, cancellationToken);
                return ResponseApi<Unit>.ResponseOk(Unit.Value);
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }
    }
}