using Application.Commons;
using FluentValidation;
using MediatR;

namespace Application.Features.Auths.VerifyToken.ConfirmEmail
{
    public class ConfirmEmailQuery : IRequest<ResponseApi<Unit>>
    {
        public string Token { get; set; }
    }

    public class ConfirmEmailQueryValidator : AbstractValidator<ConfirmEmailQuery>
    {
        public ConfirmEmailQueryValidator()
        {
            RuleFor(x => x.Token).NotEmpty().WithMessage("Token không hợp lệ");
        }
    }
}