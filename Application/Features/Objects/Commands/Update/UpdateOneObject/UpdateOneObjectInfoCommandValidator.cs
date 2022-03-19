using Application.Features.Objects.Vms;
using FluentValidation;

namespace Application.Features.Objects.Commands.Update.UpdateOneObject
{
    public sealed class UpdateOneObjectInfoCommandValidator<T> : 
        AbstractValidator<UpdateOneObjectCommand<T>> where T : ObjectVm
    {
        public UpdateOneObjectInfoCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Data).NotNull();
        }
    }
}