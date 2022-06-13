using Application.Commons;
using FluentValidation;
using MediatR;

namespace Application.Features.Profile.Commands.UpdateProfileInfo
{
    public class UpdateProfileInfoCommand : IRequest<ResponseApi<Unit>>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string DateOfBirth { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }
    }

    public class UpdateProfileInfoCommandValidator : AbstractValidator<UpdateProfileInfoCommand>
    {
        public UpdateProfileInfoCommandValidator()
        {
            RuleFor(x => x.LastName)
                .Matches("^[a-zA-Z]*$").When(x => !string.IsNullOrEmpty(x.LastName))
                .WithMessage("LastName không hợp lệ");
            RuleFor(x => x.FirstName)
                .Matches("^[a-zA-Z ]*$").When(x => !string.IsNullOrEmpty(x.FirstName))
                .WithMessage("FirstName không hợp lệ");
            RuleFor(x => x.PhoneNumber)
                .Matches("^[0-9]*$").When(x => !string.IsNullOrEmpty(x.PhoneNumber))
                .WithMessage("Số điện thoại không hợp lệ");
        }
    }
}