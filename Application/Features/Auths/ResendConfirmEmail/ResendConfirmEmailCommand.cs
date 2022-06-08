using Application.Commons;
using FluentValidation;
using MediatR;

namespace Application.Features.Auths.ResendConfirmEmail
{
    public class ResendConfirmEmailCommand : IRequest<ResponseApi<Unit>>
    {
        public string Email { get; set; }
    }

    public class ResendConfirmEmailCommandValidator : AbstractValidator<ResendConfirmEmailCommand>
    {
        public ResendConfirmEmailCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email không được trống")
                .EmailAddress().WithMessage("Email không hợp lệ");
        }
    }
}