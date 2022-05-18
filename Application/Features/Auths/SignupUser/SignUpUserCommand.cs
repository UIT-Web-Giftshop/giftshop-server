﻿using Application.Commons;
using FluentValidation;
using MediatR;

namespace Application.Features.Auths.SignupUser
{
    public class SignUpUserCommand : IRequest<ResponseApi<Unit>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class SignUpUserCommandValidator : AbstractValidator<SignUpUserCommand>
    {
        public SignUpUserCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress();
            
            RuleFor(x => x.Password)
                .NotEmpty()
                .Equal(x => x.ConfirmPassword).WithMessage("Password not matches")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters");
        }
    }
}