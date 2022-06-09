using Application.Commons;
using Domain.ViewModels.Auth;
using FluentValidation;
using MediatR;

namespace Application.Features.Auths.SigninUser
{
    public class SignInUserCommand : IRequest<ResponseApi<SignInResponseViewModel>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class SignInUserCommandValidator : AbstractValidator<SignInUserCommand>
    {
        public SignInUserCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress();
            
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters");
        }
    }
}