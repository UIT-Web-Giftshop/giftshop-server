using System;
using Application.Features.Objects.Vms;
using FluentValidation;

namespace Application.Features.Users.Vms
{
    public sealed class UserVmValidator : AbstractValidator<UserVm>, IObjectValidator
    {
        public UserVmValidator()
        {
            RuleFor(x => x.Email).NotNull();
            RuleFor(x => x.Password).NotNull();
            RuleFor(x => x.FirstName).NotNull();
            RuleFor(x => x.LastName).NotNull();
            RuleFor(x => x.IsActive).NotNull();
            RuleFor(x => x.DateOfBirth).LessThan(DateTime.Now);
        }
    }
}