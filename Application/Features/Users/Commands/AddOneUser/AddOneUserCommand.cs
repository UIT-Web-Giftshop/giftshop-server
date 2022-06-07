using Application.Commons;
using FluentValidation;
using MediatR;

namespace Application.Features.Users.Commands.AddOneUser
{
    public class AddOneUserCommand : IRequest<ResponseApi<string>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class AddOneUserCommandValidator : AbstractValidator<AddOneUserCommand>
    {
        public AddOneUserCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email không được để trống")
                .EmailAddress().WithMessage("Email không hợp lệ");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password không được để trống")
                .MinimumLength(6).WithMessage("Password phải có ít nhất 6 ký tự");
            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("LastName không được để trống")
                .Matches("^[a-zA-Z]*$").WithMessage("LastName không hợp lệ");
            RuleFor(x => x.FirstName)
                .Matches("^[a-zA-Z ]*$").When(x => !string.IsNullOrEmpty(x.FirstName))
                .WithMessage("FirstName không hợp lệ");
        }
    }
}