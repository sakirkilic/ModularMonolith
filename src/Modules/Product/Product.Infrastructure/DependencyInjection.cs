using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Product.Application.Abstractions.Authentication;
using Product.Application.Abstractions.Caching;
using Product.Application.Abstractions.Data;
using Product.Infrastructure.Authentication;
using Product.Infrastructure.Caching;
using Product.Infrastructure.Persistence;
using Product.Infrastructure.Persistence.Repositories;

namespace Product.Infrastructure
{
    // Product.Infrastructure servis kayıtlarını toplar
    public static class DependencyInjection
    {
        // Infrastructure katmanı servislerini ekler
        public static IServiceCollection AddProductInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<ProductDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("ProductDatabase"));
            });

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            services.AddMemoryCache();
            services.AddScoped<ICacheService, MemoryCacheService>();


            return services;
        }
    }
}
