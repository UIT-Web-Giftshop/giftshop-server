using System.Collections.Generic;
using ServiceStack.DataAnnotations;

namespace Application.Features.Products.Vms
{
    public class ProductVm
    {
        [Required]
        public string Sku { get; init; }
        [Required]
        public string Name { get; init; }
        public string Description { get; init; }
        public uint Quantity { get; init; }
        public double Price { get; init; }
        public List<string> Traits { get; init; }
        public string ImageUrl { get; init; }    
        public bool IsActive { get; init; } = false;
    }
}