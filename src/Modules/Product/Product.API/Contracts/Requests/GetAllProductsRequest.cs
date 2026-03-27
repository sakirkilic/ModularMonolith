namespace Product.API.Contracts.Requests
{

    // Ürün listeleme API isteğini temsil eder
    public sealed record GetAllProductsRequest(
        int Page = 1,
        int PageSize = 10,
        string? Search = null,
        decimal? MinPrice = null,
        decimal? MaxPrice = null,
        string? SortBy = null,
        string? SortDirection = null
    );
}
