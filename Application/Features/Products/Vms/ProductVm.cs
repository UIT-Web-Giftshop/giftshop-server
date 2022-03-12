using System.Collections.Generic;
using System.ComponentModel;
using FluentValidation;

namespace Application.Features.Products.Vms
{
    public class ProductVm
    {
        public string Id { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public double Price { get; set; }
        public object Detail { get; set; }
        public List<string> Traits { get; set; }
        public string ImageUrl { get; set; } 
        [DefaultValue(true)]
        public bool IsActive { get; set; }
    }

    public sealed class ProductVmValidator : AbstractValidator<ProductVm>
    {
        public ProductVmValidator()
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.Name).NotNull();
            RuleFor(x => x.Stock).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
        }
    }
}