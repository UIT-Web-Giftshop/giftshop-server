using Application.Commons;
using FluentValidation;
using MediatR;

namespace Application.Features.Auths.VerifyToken.ConfirmResetPassword
{
    public class ConfirmResetPasswordCommand : IRequest<ResponseApi<Unit>>
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public sealed class ConfirmResetPasswordCommandValidator : AbstractValidator<ConfirmResetPasswordCommand>
    {
        public ConfirmResetPasswordCommandValidator()
        {
            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("Token không hợp lệ");
            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("Mật khẩu không được trống")
                .MinimumLength(6).WithMessage("Mật khẩu phải có ít nhất 6 ký tự");
            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.NewPassword).WithMessage("Mật khẩu không trùng khớp");
        }
    }
}