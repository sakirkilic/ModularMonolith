namespace Product.API.Contracts.Requests
{
    // Ürün güncelleme API isteğini temsil eder
    public sealed record UpdateProductRequest(
        string Name,
        decimal Price,
        int StockQuantity
    );
}
