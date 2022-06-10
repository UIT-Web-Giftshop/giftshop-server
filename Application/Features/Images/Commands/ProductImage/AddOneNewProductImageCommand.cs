using Application.Commons;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Images.Commands.ProductImage
{
    public class AddOneNewProductImageCommand : IRequest<ResponseApi<string>>
    {
        public string Sku { get; set; }
        public IFormFile File { get; set; }
    }

    public class AddOneNewProductImageCommandValidator : AbstractValidator<AddOneNewProductImageCommand>
    {
        public AddOneNewProductImageCommandValidator()
        {
            RuleFor(x => x.Sku).NotEmpty();
            RuleFor(x => x.File).NotNull();
        }
    }
}