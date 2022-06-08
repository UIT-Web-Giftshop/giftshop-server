using Application.Commons;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Images.Commands.ProductImage
{
    public class AddOneProductImageCommand : IRequest<ResponseApi<string>>
    {
        public string Sku { get; set; }
        public IFormFile File { get; set; }
    }
    
    public sealed class AddProductImageCommandValidator : AbstractValidator<AddOneProductImageCommand>
    {
        public AddProductImageCommandValidator()
        {
            RuleFor(x => x.Sku).NotEmpty();
            RuleFor(x => x.File).NotNull();
        }
    }
}