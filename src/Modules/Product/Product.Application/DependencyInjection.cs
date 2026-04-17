using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Product.Application.Behaviors;

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

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }
    }
}
