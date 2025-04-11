using Application.Interfaces.DiscountAggregate;
using Application.Interfaces.ProductAggregate;
using Application.Services.DiscountAggregate;
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

            services.AddScoped<IDiscountApplication, DiscountApplication>();

            services.AddScoped<ISlideApplication, SlideApplication>();
        }
    }
}
