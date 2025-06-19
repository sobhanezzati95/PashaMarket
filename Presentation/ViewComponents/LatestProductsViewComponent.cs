using Application.Interfaces.ProductAggregate;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.ViewComponents;
public class LatestProductsViewComponent(IProductApplication productApplication) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(CancellationToken cancellationToken = default)
    {
        var productCategories = await productApplication.GetLatestProductsQuery(cancellationToken);
        return View(productCategories);
    }
}