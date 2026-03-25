namespace Product.Application.Features.Products.GetAllProducts
{
    // Ürün listeleme için dönen response modeli
    public sealed record ProductListItemResponse(
        Guid Id,
        string Name,
        decimal Price,
        int StockQuantity
    );
}
