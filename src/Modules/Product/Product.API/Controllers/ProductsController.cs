using MediatR;
using Microsoft.AspNetCore.Mvc;
using Product.API.Contracts.Requests;
using Product.Application.Features.Products.CreateProduct;
using Product.Application.Features.Products.GetAllProducts;
using Product.Application.Features.Products.GetProductById;

namespace Product.API.Controllers
{
    // Ürün işlemlerini yöneten controller
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
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllProductsRequest request)
        {
            var query = new GetAllProductsQuery(
                request.Page,
                request.PageSize,
                request.Search,
                request.MinPrice,
                request.MaxPrice);

            var products = await _sender.Send(query);

            return Ok(products);
        }

        // Id'ye göre ürün getirir
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var query = new GetProductByIdQuery(id);

            var product = await _sender.Send(query);

            return Ok(product);
        }

        // Yeni ürün oluşturur
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
    }
}