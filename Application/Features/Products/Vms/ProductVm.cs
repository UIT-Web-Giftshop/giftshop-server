using System.Collections.Generic;
using System.ComponentModel;

namespace Application.Features.Products.Vms
{
    public class ProductVm
    {
        public string Id { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public uint Stock { get; set; }
        public double Price { get; set; }
        public object Detail { get; set; }
        public List<string> Traits { get; set; }
        public string ImageUrl { get; set; } 
        [DefaultValue(true)]
        public bool IsActive { get; set; }
    }
}