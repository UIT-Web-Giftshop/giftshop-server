using Application.Commons;
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
                .NotEmpty().WithMessage("Email không được để trống")
                .EmailAddress().WithMessage("Email không hợp lệ");
            
            RuleFor(x => x.Password)
                .NotEmpty()
                .Equal(x => x.ConfirmPassword).WithMessage("Mật khẩu không được trống")
                .MinimumLength(6).WithMessage("Mật khẩu phải ít nhất 6 ký tự");
        }
    }
}