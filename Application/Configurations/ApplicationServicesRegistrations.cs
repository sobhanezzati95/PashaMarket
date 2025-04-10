using Application.Interfaces.ProductAggregate;
using Application.Services.ProductAggregate;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Configurations
{
    public static class ApplicationServicesRegistrations
    {
        public static void SetupServices(this IServiceCollection services)
        {
            services.AddScoped<IProductApplication, ProductApplication>();
            services.AddScoped<IProductCategoryApplication, ProductCategoryApplication>();
            services.AddScoped<IProductPictureApplication, ProductPictureApplication>();
            services.AddScoped<ISlideApplication, SlideApplication>();
        }
    }
}
