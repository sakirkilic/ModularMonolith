using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Application.Features.Products.GetAllProducts
{
    // Sayfalı ürün listeleme isteği için doğrulamaları yapar
    public sealed class GetAllProductsQueryValidator : AbstractValidator<GetAllProductsQuery>
    {
        public GetAllProductsQueryValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThan(0)
                .WithMessage("Sayfa numarası 0'dan büyük olmalıdır.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .WithMessage("Sayfa boyutu 0'dan büyük olmalıdır.")
                .LessThanOrEqualTo(100)
                .WithMessage("Sayfa boyutu en fazla 100 olabilir.");
        }
    }
}
