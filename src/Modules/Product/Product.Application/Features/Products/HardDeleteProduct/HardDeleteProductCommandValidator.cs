using FluentValidation;

namespace Product.Application.Features.Products.HardDeleteProduct
{
    // Fiziksel ürün silme isteği için doğrulamaları yapar
    public sealed class HardDeleteProductCommandValidator : AbstractValidator<HardDeleteProductCommand>
    {
        public HardDeleteProductCommandValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty()
                .WithMessage("Ürün kimliği boş olamaz.");
        }
    }
}
