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
        
        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetOneProductById(string id)
        {
            var result = await _mediator.Send(new GetOneProductById.Query { Id = id });
            return HandleResponseStatus(result);
        }

        [HttpGet("sku/{sku}")]
        public async Task<IActionResult> GetOneProductBySku(string sku)
        {
            var result = await _mediator.Send(new GetOneProductBySku.Query() { Sku = sku });
            return HandleResponseStatus(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddOneProduct([FromBody] ProductVm addProductVm)
        {
            var result = await _mediator.Send(new AddOneProduct.Command() { Product = addProductVm });
            return HandleResponseStatus(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOneProductInfo(string id, [FromBody] ProductVm productVm)
        {
            var result = await _mediator.Send(new UpdateOneProduct.Command() { Id = id, Product = productVm });
            return HandleResponseStatus(result);
        }

        [HttpPatch("{id}/quantity/{quantity:int}")]
        public async Task<IActionResult> UpdateOneProductQuantity(string id, uint quantity)
        {
            var result = await _mediator.Send(new PatchOneProductQuantity.Command() { Id = id, Quantity = quantity });
            return HandleResponseStatus(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOneProduct(string id)
        {
            var result = await _mediator.Send(new DeleteOneProduct.Command() { Id = id });
            return HandleResponseStatus(result);
        }
    }
}