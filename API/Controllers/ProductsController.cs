#nullable enable
using System.Threading;
using System.Threading.Tasks;
using Application.Features.Products.Commands;
using Application.Features.Products.Queries.GetOneProductById;
using Application.Features.Products.Queries.GetOneProductBySku;
using Application.Features.Products.Queries.GetPagingProducts;
using Domain.ViewModels.Product;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [AllowAnonymous]
    public class ProductsController : BaseApiController
    {
        public ProductsController(IMediator _mediator) : base(_mediator) { }

        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetOneProductById(string id)
        {
            var result = await this._mediator.Send(new GetOneProductByIdQuery() { Id = id });
            return HandleResponseStatus(result);
        }
        
        [HttpGet("sku/{sku}")]
        public async Task<IActionResult> GetOneProductBySku(string sku)
        {
            var result = await this._mediator.Send(new GetOneProductBySkuQuery() { Sku = sku });
            return HandleResponseStatus(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetPagingProducts(
            [FromQuery] GetPagingProductsQuery query,
            CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return HandleResponseStatus(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewOneProduct([FromBody] ProductDetailViewModel command)
        {
            var result = await _mediator.Send(new AddOneProductCommand {Product = command});
            return HandleResponseStatus(result);
        }

        [HttpPut("{sku}")]
        public async Task<IActionResult> UpdateOneProductInfo(
            string sku, 
            [FromBody] ProductDetailViewModel productDetailViewModel)
        {
            productDetailViewModel.Sku = sku;
            
            var result = await _mediator
                .Send(new UpdateOneProductDetailCommand { Product = productDetailViewModel });
            return HandleResponseStatus(result);
        }
        
        [HttpPatch("{id}/quantity/{stock:int}")]
        public async Task<IActionResult> UpdateOneProductStock(string id, uint stock)
        {
            var result = await _mediator
                .Send(new UpdateOneProductStockCommand { Id = id, Stock = stock });
            return HandleResponseStatus(result);
        }
        
        [HttpPatch("{id}/price/{price:double}")]
        public async Task<IActionResult> UpdateOneProductPrice(string id, double price)
        {
            var result = await _mediator
                .Send(new UpdateOneProductPriceCommand() { Id = id, Price = price });
            return HandleResponseStatus(result);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOneProduct(string id)
        {
            var result = await _mediator
                .Send(new DeleteOneProductCommand() { Id = id });
            return HandleResponseStatus(result);
        }
    }
}