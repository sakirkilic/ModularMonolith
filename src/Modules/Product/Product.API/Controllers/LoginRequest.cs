namespace Product.API.Controllers
{
    // Demo login isteğini temsil eder
    public sealed record LoginRequest(
        string Email,
        string Role
    );
}
