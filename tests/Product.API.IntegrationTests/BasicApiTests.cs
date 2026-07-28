using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace Product.API.IntegrationTests
{
    // API temel çalışıyor mu test eder
    public sealed class BasicApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public BasicApiTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Get_Should_Return404_ForUnknownEndpoint()
        {
            // Act
            var response = await _client.GetAsync("/unknown-endpoint");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
