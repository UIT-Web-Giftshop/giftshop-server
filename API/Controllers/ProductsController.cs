#nullable enable
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.Products.Commands.AddOneProduct;
using Application.Features.Products.Commands.UpdateListProductState;
using Application.Features.Products.Commands.UpdateOneProductDetail;
using Application.Features.Products.Commands.UpdateOneProductPrice;
using Application.Features.Products.Commands.UpdateOneProductState;
using Application.Features.Products.Commands.UpdateOneProductStock;
using Application.Features.Products.Queries.GetOneProductById;
using Application.Features.Products.Queries.GetOneProductBySku;
using Application.Features.Products.Queries.GetPagingProducts;
using Domain.Paging;
using Domain.ViewModels.Product;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [AllowAnonymous]
    public class ProductsController : BaseApiController
    {
        public ProductsController(IMediator _mediator) : base(_mediator)
        {
        }

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

        [HttpGet("trait/{trait}")]
        public async Task<IActionResult> GetPagingProductByTrait(
            [FromQuery] PagingRequest pagingRequest,
            [FromRoute] string trait,
            [FromQuery] [DefaultValue("price")] string sortBy,
            [FromQuery] [DefaultValue(true)] bool isDesc)
        {
            var query = new GetPagingProductByTraitQuery()
                { PagingRequest = pagingRequest, Trait = trait, SortBy = sortBy, IsDesc = isDesc };
            var data = await _mediator.Send(query);
            return HandleResponseStatus(data);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewOneProduct([FromBody] ProductDetailViewModel command)
        {
            var result = await _mediator.Send(new AddOneProductCommand { Product = command });
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

        [HttpPatch("{sku}/quantity/{stock:int}")]
        public async Task<IActionResult> UpdateOneProductStock(string sku, int stock)
        {
            var result = await _mediator
                .Send(new UpdateOneProductStockCommand { Sku = sku, Stock = stock });
            return HandleResponseStatus(result);
        }

        [HttpPatch("{sku}/price/{price:double}")]
        public async Task<IActionResult> UpdateOneProductPrice(string sku, double price)
        {
            var result = await _mediator
                .Send(new UpdateOneProductPriceCommand { Sku = sku, Price = price });
            return HandleResponseStatus(result);
        }

        [HttpPatch("state/{state}/{sku}")]
        public async Task<IActionResult> UpdateOneProductState(string sku, ProductState state)
        {
            var result = await _mediator
                .Send(new UpdateOneProductStateCommand() { Sku = sku, State = state });
            return HandleResponseStatus(result);
        }

        /// <summary>
        /// Use sku as list
        /// </summary>
        /// <param name="skus"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpPatch("state/{state}/list-sku")]
        public async Task<IActionResult> UpdateListProductState(
            [FromBody] HashSet<string> skus,
            [FromRoute] ProductState state)
        {
            var result = await _mediator.Send(new UpdateListProductStateCommand() { Skus = skus, State = state });
            return HandleResponseStatus(result);
        }
    }
}