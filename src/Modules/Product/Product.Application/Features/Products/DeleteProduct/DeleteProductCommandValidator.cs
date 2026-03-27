using FluentValidation;

namespace Product.Application.Features.Products.DeleteProduct
{
    // Ürün silme isteği için doğrulamaları yapar
    public sealed class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty()
                .WithMessage("Ürün kimliği boş olamaz.");
        }
    }
}
