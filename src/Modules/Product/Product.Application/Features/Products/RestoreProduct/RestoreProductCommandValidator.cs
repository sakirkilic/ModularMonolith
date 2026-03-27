using FluentValidation;

namespace Product.Application.Features.Products.RestoreProduct
{
    // Ürün geri yükleme isteği için doğrulamaları yapar
    public sealed class RestoreProductCommandValidator : AbstractValidator<RestoreProductCommand>
    {
        public RestoreProductCommandValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty()
                .WithMessage("Ürün kimliği boş olamaz.");
        }
    }
}
