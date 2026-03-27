using FluentValidation;

namespace Product.Application.Features.Products.UpdateProduct
{
    // Ürün güncelleme isteği için doğrulamaları yapar
    public sealed class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty()
                .WithMessage("Ürün kimliği boş olamaz.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Ürün adı boş olamaz.")
                .MaximumLength(200)
                .WithMessage("Ürün adı 200 karakterden uzun olamaz.");

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("Ürün fiyatı sıfırdan büyük olmalıdır.");

            RuleFor(x => x.StockQuantity)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Stok miktarı negatif olamaz.");
        }
    }
}
