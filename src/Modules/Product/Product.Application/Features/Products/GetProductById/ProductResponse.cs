namespace Product.Application.Features.Products.GetProductById
{
    // Ürün detayını dış dünyaya dönen response modeli
    public sealed record ProductResponse(
        Guid Id,
        string Name,
        decimal Price,
        int StockQuantity
    );
}
