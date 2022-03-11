using System.Collections.Generic;
using Domain.Entities;

namespace Web.Tests
{
    public static class ProductResource
    {
        public static List<Product> ListProducts()
        {
            return new List<Product>()
            {
                new Product()
                {
                    Id = "6227538b5f2c8331916327cb",
                    Sku = "xUnitTest01",
                    Name = "xUnit",
                    Description = "xUnit Test description",
                    Stock = 10
                },
                new Product()
                {
                    Id = "6227538b5f2c8331916327cc",
                    Sku = "xUnitTest02",
                    Name = "xUnit product 02",
                    Description = "xUnit Test description",
                    Stock = 11,
                    Price = 11.80
                },
                new Product()
                {
                    Id = "6227538b5f2c8331916327cd",
                    Sku = "xUnitTest03",
                    Name = "xUnit product 03",
                    Description = "xUnit Test description",
                    Stock = 12,
                    Price = 12.80
                }
            };
        }
    }
}