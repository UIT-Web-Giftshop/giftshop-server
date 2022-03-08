using System.Threading.Tasks;
using Application.Features.Products.Commands;
using Application.Features.Products.Queries;
using Application.Features.Products.Vms;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        public ProductsController(IMediator mediator) : base(mediator)
        {
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOneProductById(string id)
        {
            var result = await _mediator.Send(new GetOneProductById.Query { Id = id });
            return HandleResponseStatus(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddOneProduct([FromBody] ProductVm addProductVm)
        {
            var result = await _mediator.Send(new AddOneProduct.Command() { ProductVm = addProductVm });
            return HandleResponseStatus(result);
        }
    }
}