using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Models;
using Infrastructure.Interfaces.Repositories;
using Infrastructure.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MongoDB.Bson;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestsController : ControllerBase
    {
        private readonly IHostEnvironment _hostEnv;
        private readonly IProductRepository _productRepository;
        private readonly IMailService _mailService;

        public TestsController(IHostEnvironment hostEnv, IProductRepository productRepository, IMailService mailService)
        {
            _hostEnv = hostEnv;
            _productRepository = productRepository;
            _mailService = mailService;
        }

        [HttpPost("seed-products")]
        public async Task<IActionResult> SeedProduct()
        {
            if (_hostEnv.IsDevelopment())
            {
                // add products
                try
                {
                    var products = InstantiateProducts();
                    foreach (var item in products)
                    {
                        await _productRepository.InsertAsync(item);
                    }
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost("send-email")]
        public async Task<IActionResult> SendEmail([FromForm] MailRequestModel requestModel)
        {
            try
            {
                await _mailService.SendAsync(requestModel);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("send-email-template")]
        public async Task<IActionResult> SendEmailWithTemplate(
            [FromForm] string templateName,
            [FromForm] string to,
            [FromForm] List<string> args)
        {
            try
            {
                await _mailService.SendWithTemplate(templateName, to, args.ToArray());
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Init a list of products for seeding
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<Product> InstantiateProducts()
        {
            return new List<Product>()
            {
                new Product()
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Sku = "test01",
                    Name = "gift demo 01",
                    Description = "a initial gift demo product",
                    Stock = 10,
                    Price = 12.30,
                    Detail = new
                    {
                        Color = "red",
                        Size = "M"
                    },
                    Traits = new List<string>() { "gift", "demo", "friend" },
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new Product()
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Sku = "test02",
                    Name = "gift demo 02",
                    Description = "the perfect gift for family",
                    Stock = 34,
                    Price = 25.50,
                    Detail = new
                    {
                        Meterial = "fabric",
                        Color = "blue",
                    },
                    Traits = new List<string>() { "gift", "demo", "family" },
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new Product()
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Sku = "test03",
                    Name = "gift demo 03",
                    Description = "A sweet valentine gift for girlfriend",
                    Stock = 54,
                    Price = 50.25,
                    Traits = new List<string>() { "gift", "demo", "valentine", "girl" },
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
            };
        }
    }
}