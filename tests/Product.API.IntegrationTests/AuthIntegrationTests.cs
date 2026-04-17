using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Product.API.IntegrationTests;

// Auth akışını test eder
public sealed class AuthIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public AuthIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Login_Should_ReturnToken()
    {
        // Arrange
        var request = new
        {
            email = "admin@test.com",
            role = "admin"
        };

        var content = new StringContent(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await _client.PostAsync("/api/auth/login", content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = await response.Content.ReadAsStringAsync();

        Assert.Contains("token", json);
    }

    [Fact]
    public async Task Me_Should_Return401_When_NotAuthenticated()
    {
        // Act
        var response = await _client.GetAsync("/api/auth/me");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Me_Should_ReturnUser_When_Authenticated()
    {
        // 1) login ol
        var loginRequest = new
        {
            email = "admin@test.com",
            role = "admin"
        };

        var content = new StringContent(
            JsonSerializer.Serialize(loginRequest),
            Encoding.UTF8,
            "application/json");

        var loginResponse = await _client.PostAsync("/api/auth/login", content);

        var loginJson = await loginResponse.Content.ReadAsStringAsync();

        var jsonDoc = JsonDocument.Parse(loginJson);
        var token = jsonDoc.RootElement.GetProperty("token").GetString();

        // 2) token'ı ekle
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        // 3) endpoint çağır
        var response = await _client.GetAsync("/api/auth/me");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task CreateProduct_Should_Return403_When_UserHasNoPermission()
    {
        // login → user role
        var loginRequest = new
        {
            email = "user@test.com",
            role = "user"
        };

        var content = new StringContent(
            JsonSerializer.Serialize(loginRequest),
            Encoding.UTF8,
            "application/json");

        var loginResponse = await _client.PostAsync("/api/auth/login", content);

        var loginJson = await loginResponse.Content.ReadAsStringAsync();

        var jsonDoc = JsonDocument.Parse(loginJson);
        var token = jsonDoc.RootElement.GetProperty("token").GetString();

        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        // ürün oluşturmayı dene
        var productRequest = new
        {
            name = "Test",
            price = 100,
            stockQuantity = 5
        };

        var productContent = new StringContent(
            JsonSerializer.Serialize(productRequest),
            Encoding.UTF8,
            "application/json");

        var response = await _client.PostAsync("/api/products", productContent);

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
}