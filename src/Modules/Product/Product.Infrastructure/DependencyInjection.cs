using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Product.Application.Abstractions.Data;
using Product.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            return services;
        }
    }
}
