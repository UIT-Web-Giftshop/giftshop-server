using Application.Commons;
using FluentValidation;
using MediatR;

namespace Application.Features.Wishlists.Commands.UpdateOneWishlist
{
    public class UpdateAddWishlistItemCommand : IRequest<ResponseApi<Unit>>
    {
        public string Sku { get; set; }
    }

    public class UpdateAddWishlistCommandValidator : AbstractValidator<UpdateAddWishlistItemCommand>
    {
        public UpdateAddWishlistCommandValidator()
        {
            RuleFor(x => x.Sku).NotEmpty().WithMessage("Mã sản phẩm không hợp lệ");
        }
    }
}