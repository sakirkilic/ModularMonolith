using BuildingBlocks.Domain;

namespace Product.Domain.ValueObjects
{
    // Product benzersiz kimliğini temsil eder
    public sealed class ProductId : ValueObject
    {
        public Guid Value { get; }

        private ProductId(Guid value)
        {
            Value = value;
        }

        public static ProductId Create(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException(
                    "ProductId boş olamaz. / ProductId cannot be empty.",
                    nameof(value));
            }

            return new ProductId(value);
        }

        public static ProductId New()
        {
            return new ProductId(Guid.NewGuid());
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
