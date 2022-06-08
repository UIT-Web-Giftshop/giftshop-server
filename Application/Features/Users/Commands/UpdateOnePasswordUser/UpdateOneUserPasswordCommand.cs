using Application.Commons;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Features.Users.Commands.UpdateOnePasswordUser
{
    public class UpdateOneUserPasswordCommand : IRequest<ResponseApi<Unit>>
    {
        [FromRoute] public string Id { get; set; }
        [FromBody] public string Password { get; set; }
    }

    public class UpdateOneUserPasswordCommandValidator : AbstractValidator<UpdateOneUserPasswordCommand>
    {
        public UpdateOneUserPasswordCommandValidator()
        {
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password không được để trống")
                .MinimumLength(6).WithMessage("Password phải có ít nhất 6 ký tự");
        }
    }
}