﻿using System.Collections.Generic;
using System.ComponentModel;

namespace Domain.ViewModels.Product
{
    public class ProductDetailViewModel
    {
        public string Sku { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Stock { get; set; }

        public double Price { get; set; }

        public Dictionary<string, string> Detail { get; set; }

        public List<string> Traits { get; set; }

        public string ImageUrl { get; set; }

        [DefaultValue(true)] public bool IsActive { get; set; }
    }
}