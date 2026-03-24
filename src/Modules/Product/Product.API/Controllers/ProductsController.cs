using MediatR;
using Microsoft.AspNetCore.Mvc;
using Product.Application.Features.Products.CreateProduct;

namespace Product.API.Controllers
{
    // Ürün işlemlerini yöneten controller
    [ApiController]
    [Route("api/products")]
    public sealed class ProductsController : ControllerBase
    {
        private readonly ISender _sender;

        // MediatR üzerinden handler çağırmak için
        public ProductsController(ISender sender)
        {
            _sender = sender;
        }

        // Yeni ürün oluşturur
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductCommand command)
        {
            var productId = await _sender.Send(command);

            return Ok(productId);
        }
    }
}