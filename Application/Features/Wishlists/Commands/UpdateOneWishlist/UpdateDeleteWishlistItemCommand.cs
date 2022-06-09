using FluentValidation;

namespace Application.Features.Wishlists.Commands.UpdateOneWishlist
{
    public class UpdateDeleteWishlistItemCommand : UpdateAddWishlistItemCommand
    {
        
    }

    public class UpdateDeleteWishlistItemCommandValidator : AbstractValidator<UpdateDeleteWishlistItemCommand>
    {
        public UpdateDeleteWishlistItemCommandValidator()
        {
            RuleFor(x => x.Sku).NotEmpty().WithMessage("Mã sản phẩm không hợp lệ");
        }
    }
}