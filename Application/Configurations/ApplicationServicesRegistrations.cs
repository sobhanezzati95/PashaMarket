﻿using Application.Interfaces.ContactUsAggregate;
using Application.Interfaces.DiscountAggregate;
using Application.Interfaces.OrderAggregate;
using Application.Interfaces.ProductAggregate;
using Application.Interfaces.UserAggregate;
using Application.Services.ContactUsAggregate;
using Application.Services.DiscountAggregate;
using Application.Services.OrderAggregate;
using Application.Services.ProductAggregate;
using Application.Services.UserAggregate;
using Framework.Application;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Configurations;
public static class ApplicationServicesRegistrations
{
    public static void SetupServices(this IServiceCollection services)
    {
        services.AddScoped<IProductApplication, ProductApplication>();
        services.AddScoped<IProductCategoryApplication, ProductCategoryApplication>();
        services.AddScoped<IProductPictureApplication, ProductPictureApplication>();
        services.AddScoped<IProductFeatureApplication, ProductFeatureApplication>();
        services.AddScoped<IDiscountApplication, DiscountApplication>();
        services.AddScoped<ISlideApplication, SlideApplication>();
        services.AddScoped<IOrderApplication, OrderApplication>();
        services.AddScoped<IAuthenticationHelper, AuthenticationHelper>();
        services.AddScoped<IUserApplication, UserApplication>();
        services.AddScoped<ICartCalculatorService, CartCalculatorService>();
        services.AddSingleton<ICartService, CartService>();
        services.AddScoped<IContactUsApplication, ContactUsApplication>();
    }
}