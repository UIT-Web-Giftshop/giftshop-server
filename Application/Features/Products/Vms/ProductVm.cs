using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.Products.Vms
{
    public class ProductVm
    {
        [Required]
        public string Sku { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public uint Quantity { get; set; }
        public double Price { get; set; }
        public List<string> Traits { get; set; }
        public string ImageUrl { get; set; } 
        [DefaultValue(false)]
        public bool IsActive { get; set; }
    }
}