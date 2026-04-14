using MediatR;
using Moq;
using Product.Application.Abstractions.Caching;
using Product.Application.Behaviors;

namespace Product.Application.UnitTests
{
    // CachingBehavior davranışlarını test eder
    public sealed class CachingBehaviorTests
    {
        [Fact]
        public async Task Handle_Should_ReturnCachedResponse_When_CacheHasValue()
        {
            // Arrange
            var cacheServiceMock = new Mock<ICacheService>();

            var request = new TestCacheableQuery();
            var cachedResponse = "cache-response";

            cacheServiceMock
                .Setup(x => x.GetAsync<string>(request.CacheKey))
                .ReturnsAsync(cachedResponse);

            var behavior = new CachingBehavior<TestCacheableQuery, string>(cacheServiceMock.Object);

            var nextCalled = false;

            RequestHandlerDelegate<string> next = cancellationToken =>
            {
                nextCalled = true;
                return Task.FromResult("handler-response");
            };

            // Act
            var result = await behavior.Handle(request, next, CancellationToken.None);

            // Assert
            Assert.Equal("cache-response", result);
            Assert.False(nextCalled);

            cacheServiceMock.Verify(
                x => x.SetAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TimeSpan>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_Should_CallNextAndSetCache_When_CacheDoesNotHaveValue()
        {
            // Arrange
            var cacheServiceMock = new Mock<ICacheService>();

            var request = new TestCacheableQuery();

            cacheServiceMock
                .Setup(x => x.GetAsync<string>(request.CacheKey))
                .ReturnsAsync((string?)null);

            var behavior = new CachingBehavior<TestCacheableQuery, string>(cacheServiceMock.Object);

            var nextCalled = false;

            RequestHandlerDelegate<string> next = cancellationToken =>
            {
                nextCalled = true;
                return Task.FromResult("handler-response");
            };

            // Act
            var result = await behavior.Handle(request, next, CancellationToken.None);

            // Assert
            Assert.Equal("handler-response", result);
            Assert.True(nextCalled);

            cacheServiceMock.Verify(
                x => x.SetAsync(request.CacheKey, "handler-response", request.Expiration),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Should_BypassCache_When_RequestIsNotCacheable()
        {
            // Arrange
            var cacheServiceMock = new Mock<ICacheService>();

            var request = new NonCacheableQuery();

            var behavior = new CachingBehavior<NonCacheableQuery, string>(cacheServiceMock.Object);

            var nextCalled = false;

            RequestHandlerDelegate<string> next = cancellationToken =>
            {
                nextCalled = true;
                return Task.FromResult("handler-response");
            };

            // Act
            var result = await behavior.Handle(request, next, CancellationToken.None);

            // Assert
            Assert.Equal("handler-response", result);
            Assert.True(nextCalled);

            cacheServiceMock.Verify(
                x => x.GetAsync<string>(It.IsAny<string>()),
                Times.Never);

            cacheServiceMock.Verify(
                x => x.SetAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TimeSpan>()),
                Times.Never);
        }

        // Cache'lenebilir test query'si
        private sealed record TestCacheableQuery : ICacheableQuery, IRequest<string>
        {
            public string CacheKey => "test-key";
            public TimeSpan Expiration => TimeSpan.FromMinutes(5);
        }

        // Cache'lenmeyen test query'si
        private sealed record NonCacheableQuery : IRequest<string>;
    }
}
