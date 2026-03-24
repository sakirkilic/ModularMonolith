namespace Product.API.Contracts.Requests
{
    // Ürün oluşturma API isteğini temsil eder
    public sealed record CreateProductRequest(
        string Name,
        decimal Price,
        int StockQuantity
    );
}
