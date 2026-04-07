using MediatR;
using Microsoft.AspNetCore.Mvc;
using Product.API.Contracts.Requests;
using Product.Application.Features.Products.CreateProduct;
using Product.Application.Features.Products.DeleteProduct;
using Product.Application.Features.Products.GetAllProducts;
using Product.Application.Features.Products.GetProductById;
using Product.Application.Features.Products.HardDeleteProduct;
using Product.Application.Features.Products.RestoreProduct;
using Product.Application.Features.Products.UpdateProduct;
using Microsoft.AspNetCore.Authorization;

namespace Product.API.Controllers
{
    // Ürün işlemlerini yöneten controller
    [Authorize]
    [ApiController]
    [Route("api/products")]
    public sealed class ProductsController : ControllerBase
    {
        private readonly ISender _sender;

        public ProductsController(ISender sender)
        {
            _sender = sender;
        }

        // Ürünleri sayfalı olarak getirir
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllProductsRequest request)
        {
            var query = new GetAllProductsQuery(
                request.Page,
                request.PageSize,
                request.Search,
                request.MinPrice,
                request.MaxPrice,
                request.SortBy,
                request.SortDirection);

            var products = await _sender.Send(query);

            return Ok(products);
        }

        // Id'ye göre ürün getirir
        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var query = new GetProductByIdQuery(id);

            var product = await _sender.Send(query);

            return Ok(product);
        }

        // Yeni ürün oluşturur
        [Authorize(Policy = "ManageProducts")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
        {
            var command = new CreateProductCommand(
                request.Name,
                request.Price,
                request.StockQuantity);

            var productId = await _sender.Send(command);

            return Ok(productId);
        }

        // Mevcut ürünü günceller
        [Authorize(Policy = "ManageProducts")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductRequest request)
        {
            var command = new UpdateProductCommand(
                id,
                request.Name,
                request.Price,
                request.StockQuantity);

            await _sender.Send(command);

            return NoContent();
        }

        // Mevcut ürünü silinmiş olarak işaretler
        [Authorize(Policy = "ManageProducts")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteProductCommand(id);

            await _sender.Send(command);

            return NoContent();
        }

        // Silinmiş ürünü tekrar aktif hale getirir
        [Authorize(Policy = "ManageProducts")]
        [HttpPost("{id:guid}/restore")]
        public async Task<IActionResult> Restore(Guid id)
        {
            var command = new RestoreProductCommand(id);

            await _sender.Send(command);

            return NoContent();
        }
        // Mevcut ürünü fiziksel olarak siler
        [Authorize(Policy = "HardDeleteProducts")]
        [HttpDelete("{id:guid}/hard")]
        public async Task<IActionResult> HardDelete(Guid id)
        {
            var command = new HardDeleteProductCommand(id);

            await _sender.Send(command);

            return NoContent();
        }

    }
}