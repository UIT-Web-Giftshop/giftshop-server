using System.Threading.Tasks;
using Application.Features.Products.Queries;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOneProductById(string id)
        {
            var result = await _mediator.Send(new GetOneProductById.Query() { Id = id });
            return HandleResponseStatus(result);
        }
    }
}