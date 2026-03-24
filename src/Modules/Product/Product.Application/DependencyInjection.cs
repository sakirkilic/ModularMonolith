using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Product.Application
{
    // Product.Application servis kayıtlarını toplar
    public static class DependencyInjection
    {
        // Application katmanı servislerini ekler
        public static IServiceCollection AddProductApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            });

            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

            return services;
        }
    }
}
