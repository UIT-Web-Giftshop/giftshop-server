using Application.Commons;
using FluentValidation;
using MediatR;

namespace Application.Features.Profile.Commands.ChangePassword
{
    public class ChangePasswordCommand : IRequest<ResponseApi<Unit>>
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public sealed class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(x => x.OldPassword)
                .NotEmpty().WithMessage("Mật khẩu cũ bị trống");
            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("Mật khẩu mới bị trống")
                .MinimumLength(6).WithMessage("Mật khẩu mới phải có ít nhất 6 ký tự")
                .NotEqual(x => x.OldPassword).WithMessage("Mật khẩu mới không được trùng với mật khẩu cũ");
            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.NewPassword).WithMessage("Mật khẩu xác nhận không khớp");
        }
    }
}