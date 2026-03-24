using BuildingBlocks.Domain;
using Product.Domain.Errors;
using Product.Domain.Events;
using Product.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Domain.Entities
{
    // Product modülünün aggregate root'u
    public sealed class Product : BaseEntity, IAggregateRoot
    {
        public ProductId Id { get; private set; } = default!;
        public string Name { get; private set; } = string.Empty;

        private Product()
        {
        }

        private Product(ProductId id, string name)
        {
            Id = id;
            Name = name;
        }

        public static Result<Product> Create(string name)
        {
            var validationResult = ValidateName(name);
            if (validationResult.IsFailure)
            {
                return Result<Product>.Failure(validationResult.Error);
            }

            var product = new Product(ProductId.New(), name.Trim());

            product.AddDomainEvent(new ProductCreatedDomainEvent(product.Id));

            return Result<Product>.Success(product);
        }

        public Result Rename(string name)
        {
            var validationResult = ValidateName(name);
            if (validationResult.IsFailure)
            {
                return validationResult;
            }

            Name = name.Trim();

            return Result.Success();
        }

        private static Result ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return Result.Failure(ProductErrors.NameEmpty);
            }

            if (name.Trim().Length > 200)
            {
                return Result.Failure(ProductErrors.NameTooLong);
            }

            return Result.Success();
        }
    }
}
